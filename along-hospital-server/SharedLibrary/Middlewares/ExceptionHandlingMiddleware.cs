using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Utils.ParseExceptions;
using System.Diagnostics;

namespace SharedLibrary.Middlewares
{
    public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (OperationCanceledException)
            {
                if (!context.Response.HasStarted)
                {
                    var r = Result.FailErrors(["Request was canceled"], "Canceled", StatusCodes.Status499ClientClosedRequest);
                    await Execute(context, r);
                }
            }
            catch (ValidationFailureException ex)
            {
                IActionResult result = ex.FieldErrors.Any()
                    ? Result.FailFieldErrors(ex.FieldErrors, "Validation failed", StatusCodes.Status400BadRequest)
                    : Result.FailErrors(ex.GlobalErrors.Any() ? ex.GlobalErrors : ["Validation failed"], "Validation failed", StatusCodes.Status400BadRequest);

                await Execute(context, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                await Execute(context, Result.FailError(new[] { ex.Message }, ex.Message, StatusCodes.Status401Unauthorized));
            }
            catch (DataNotFoundException ex)
            {
                await Execute(context, Result.FailError(new[] { ex.Message }, ex.Message, StatusCodes.Status404NotFound));
            }
            catch (DataConflictException ex)
            {
                await Execute(context, Result.FailError(new[] { ex.Message }, ex.Message, StatusCodes.Status409Conflict));
            }
            catch (ForbiddenException ex)
            {
                await Execute(context, Result.FailError(new[] { ex.Message }, ex.Message, StatusCodes.Status403Forbidden));
            }
            catch (InvalidDataException ex)
            {
                await Execute(context, Result.FailError(new[] { ex.Message }, ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (ArgumentNullException ex)
            {
                await Execute(context, Result.FailError(new[] { ex.Message }, ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (TimeoutException ex)
            {
                await Execute(context, Result.FailError(new[] { ex.Message }, "Service request timeout", StatusCodes.Status504GatewayTimeout));
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException or InvalidOperationException)
            {
                IActionResult result;

                if (ex.InnerException is InvalidOperationException innerEx
                    && innerEx.Message.Contains("FOREIGN KEY constraint failed"))
                {
                    var parsed = SqlExceptionParser.ParseSoftDeleteForeignKeyViolation(ex.Message);
                    if (parsed.Field != null && parsed.RelatedEntity != null)
                    {
                        var fieldErrors = new Dictionary<string, string>
                        {
                            { parsed.Field, $"The specified {parsed.RelatedEntity} does not exist" }
                        };
                        result = Result.FailFieldErrors(fieldErrors, "Invalid reference", StatusCodes.Status400BadRequest);
                    }
                    else
                    {
                        result = Result.FailError(
                            new[] { "A related record was not found" },
                            "Invalid reference",
                            StatusCodes.Status400BadRequest
                        );
                    }
                }
                else if (ex.InnerException is InvalidOperationException
                         && ex.InnerException.Message.Contains("Cannot delete"))
                {
                    result = Result.FailError(
                        new[] { ex.Message },
                        ex.Message,
                        StatusCodes.Status409Conflict
                    );
                }
                else if (ex.InnerException is SqlException sqlEx)
                {
                    result = HandleSqlException(sqlEx);
                }
                else
                {
                    result = Result.FailError(new[] { ex.Message }, "Database error", StatusCodes.Status500InternalServerError);
                }

                await Execute(context, result);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("cannot be tracked because another instance"))
            {
                var parsed = SqlExceptionParser.ParseEntityTrackingException(ex.Message);

                if (parsed.EntityType != null && parsed.Keys != null)
                {
                    var keysString = string.Join(", ", parsed.Keys);
                    var message = $"Duplicate {parsed.EntityType} entity detected. An entity with the same {keysString} is already being processed";

                    await Execute(context, Result.FailError(
                        new[] { message },
                        "Duplicate entity tracking",
                        StatusCodes.Status409Conflict
                    ));
                }
                else
                {
                    await Execute(context, Result.FailError(
                        new[] { "Cannot process duplicate data. The same record is already being tracked in the system. Please check your data and try again" },
                        "Duplicate entity tracking",
                        StatusCodes.Status409Conflict
                    ));
                }
            }
            catch (InvalidOperationException ex)
            {
                await Execute(context, Result.FailError(
                    new[] { ex.Message },
                    "Invalid operation",
                    StatusCodes.Status503ServiceUnavailable
                ));
            }
            catch (AutoMapperMappingException ex)
            {
                var parsed = AutoMappingExceptionParser.Parse(ex);

                if (!string.IsNullOrWhiteSpace(parsed.DestinationMember))
                {
                    var fieldErrors = new Dictionary<string, string>
                    {
                        { parsed.DestinationMember!, "Mapping failed while transforming data to target model" }

                    };
                    await Execute(context, Result.FailFieldErrors(fieldErrors, "Mapping failed: one or more fields could not be mapped correctly", StatusCodes.Status400BadRequest));
                }
                else
                {
                    var errors = new List<string>
                    {
                        ex.Message
                    };

                    await Execute(context, Result.FailErrors(errors, "Mapping failed: unable to complete data mapping operation", StatusCodes.Status400BadRequest));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);
                await Execute(context, Result.FailError(new[] { ex.Message }, $"Internal server error: {ex.Message}", StatusCodes.Status500InternalServerError));
            }
        }

        private static async Task Execute(HttpContext context, IActionResult result)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            context.Response.Headers["x-trace-id"] = traceId;

            var actionContext = new ActionContext { HttpContext = context };
            await result.ExecuteResultAsync(actionContext);
        }

        private static IActionResult HandleSqlException(SqlException sqlEx)
        {
            const int uniqueConstraintViolation = 2601;
            const int uniqueIndexViolation = 2627;
            const int foreignKeyViolation = 547;

            switch (sqlEx.Number)
            {
                case uniqueConstraintViolation:
                case uniqueIndexViolation:
                    var uniqueDetails = SqlExceptionParser.ParseUniqueConstraintViolation(sqlEx.Message);
                    if (uniqueDetails.Field != null)
                    {
                        var fieldErrors = new Dictionary<string, string>
                        {
                            { uniqueDetails.Field, $"A value of '{uniqueDetails.Value}' already exists for {uniqueDetails.Field}" }
                        };
                        return Result.FailFieldErrors(fieldErrors, "Duplicate value", StatusCodes.Status409Conflict);
                    }
                    return Result.FailError(new[] { "A similar record already exists" }, "Duplicate value", StatusCodes.Status409Conflict);

                case foreignKeyViolation:
                    var fkDetails = SqlExceptionParser.ParseForeignKeyConstraintViolation(sqlEx.Message);
                    if (fkDetails.Field != null)
                    {
                        if (fkDetails.Operation == "DELETE")
                        {
                            var deleteFieldErrors = new Dictionary<string, string>
                            {
                                { fkDetails.Field, $"Cannot delete because it is being referenced by {fkDetails.RelatedTable}" }
                            };
                            return Result.FailFieldErrors(deleteFieldErrors, "Delete restricted", StatusCodes.Status409Conflict);
                        }
                        var fkFieldErrors = new Dictionary<string, string>
                        {
                            { fkDetails.Field, $"The specified {fkDetails.RelatedTable} does not exist" }
                        };
                        return Result.FailFieldErrors(fkFieldErrors, "Invalid reference", StatusCodes.Status400BadRequest);
                    }

                    return Result.FailError(new[] { "A related record was not found" }, "Invalid reference", StatusCodes.Status400BadRequest);

                default:
                    return Result.FailError(new[] { "A database error occurred" }, $"Database error: {sqlEx.Message}", StatusCodes.Status500InternalServerError);
            }
        }
    }
}

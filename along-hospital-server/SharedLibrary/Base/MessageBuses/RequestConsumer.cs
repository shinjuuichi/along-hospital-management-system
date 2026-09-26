using AutoMapper;
using MassTransit;
using MessageBroker.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Utils.ParseExceptions;

namespace SharedLibrary.Base.MessageBuses
{
    public abstract class RequestConsumer<TRequest, TResponse> : IConsumer<TRequest>
        where TRequest : BaseEvent
        where TResponse : BaseContract, new()
    {
        public async Task Consume(ConsumeContext<TRequest> context)
        {
            try
            {
                var result = await this.Handle(context);
                await context.RespondAsync(result);
            }
            catch (OperationCanceledException)
            {
                await context.RespondAsync(new TResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Request was canceled",
                    ErrorCode = "Canceled",
                    Errors = ["Request was canceled"]
                });
            }
            catch (ValidationFailureException ex)
            {
                var response = new TResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Validation failed",
                    ErrorCode = "BadRequest"
                };
                if (ex.FieldErrors.Any())
                {
                    response = response with { FieldErrors = new Dictionary<string, string>(ex.FieldErrors) };
                }
                else
                {
                    var errors = ex.GlobalErrors.Any() ? ex.GlobalErrors : ["Validation failed"];
                    response = response with { Errors = errors.ToList() };
                }

                await context.RespondAsync(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                await context.RespondAsync(new TResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "Unauthorized",
                    Errors = [ex.Message]
                });
            }
            catch (DataNotFoundException ex)
            {
                await context.RespondAsync(new TResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "NotFound",
                    Errors = [ex.Message]
                });
            }
            catch (DataConflictException ex)
            {
                await context.RespondAsync(new TResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "Conflict",
                    Errors = [ex.Message]
                });
            }
            catch (InvalidDataException ex)
            {
                await context.RespondAsync(new TResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = "BadRequest",
                    Errors = [ex.Message]
                });
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx)
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
                            await context.RespondAsync(new TResponse
                            {
                                IsSuccess = false,
                                ErrorMessage = "Duplicate value",
                                ErrorCode = "Conflict",
                                FieldErrors = new Dictionary<string, string>
                                {
                                    { uniqueDetails.Field, $"A value of '{uniqueDetails.Value}' already exists for {uniqueDetails.Field}." }
                                }
                            });
                        }
                        else
                        {
                            await context.RespondAsync(new TResponse
                            {
                                IsSuccess = false,
                                ErrorMessage = "Duplicate value",
                                ErrorCode = "Conflict",
                                Errors = ["A similar record already exists."]
                            });
                        }
                        break;

                    case foreignKeyViolation:
                        var fkDetails = SqlExceptionParser.ParseForeignKeyConstraintViolation(sqlEx.Message);
                        if (fkDetails.Field != null)
                        {
                            await context.RespondAsync(new TResponse
                            {
                                IsSuccess = false,
                                ErrorMessage = "Invalid reference",
                                ErrorCode = "BadRequest",
                                Errors = [$"Invalid value for {fkDetails.Field}. The specified {fkDetails.RelatedTable} does not exist."]
                            });
                        }
                        else
                        {
                            await context.RespondAsync(new TResponse
                            {
                                IsSuccess = false,
                                ErrorMessage = "Invalid reference",
                                ErrorCode = "BadRequest",
                                Errors = ["A related record was not found."]
                            });
                        }
                        break;

                    default:
                        await context.RespondAsync(new TResponse
                        {
                            IsSuccess = false,
                            ErrorMessage = $"Database error: {sqlEx.Message}",
                            ErrorCode = "InternalServerError",
                            Errors = ["A database error occurred."]
                        });
                        break;
                }
            }
            catch (AutoMapperMappingException ex)
            {
                var parsed = AutoMappingExceptionParser.Parse(ex);
                if (!string.IsNullOrWhiteSpace(parsed.DestinationMember))
                {
                    await context.RespondAsync(new TResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Mapping failed: one or more fields could not be mapped correctly",
                        ErrorCode = "BadRequest",
                        FieldErrors = new Dictionary<string, string>
                        {
                            { parsed.DestinationMember!, "Mapping failed while transforming data to target model" }
                        }
                    });
                }
                else
                {
                    await context.RespondAsync(new TResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Mapping failed: unable to complete data mapping operation",
                        ErrorCode = "BadRequest",
                        Errors = [ex.Message]
                    });
                }
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new TResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Internal server error: {ex.Message}",
                    ErrorCode = "InternalServerError",
                    Errors = [ex.Message]
                });
            }
        }

        protected abstract Task<TResponse> Handle(ConsumeContext<TRequest> context);
    }
}
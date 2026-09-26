using MessageBroker.Abstractions;
using SharedLibrary.Commons.Exceptions;

namespace SharedLibrary.Base.MessageBuses
{
    public static class HandlingContractException
    {
        /// <summary>
        /// Throws appropriate exception if the contract indicates failure, using structured error info when available.
        /// </summary>
        public static void EnsureSuccess(this BaseContract contract)
        {
            if (!contract.IsSuccess)
            {
                if (contract.FieldErrors != null && contract.FieldErrors.Any())
                {
                    throw new ValidationFailureException(contract.FieldErrors);
                }

                if (contract.Errors != null && contract.Errors.Any())
                {
                    throw new ValidationFailureException(contract.Errors);
                }

                var code = contract.ErrorCode?.ToLowerInvariant();
                var message = contract.ErrorMessage ?? "Request failed";

                throw code switch
                {
                    "unauthorized" => new UnauthorizedAccessException(message),
                    "notfound" => new DataNotFoundException(message),
                    "conflict" or "duplicate" => new DataConflictException(message),
                    "badrequest" => new InvalidDataException(message),
                    _ => new Exception(message),
                };
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace SharedLibrary.Commons.Results
{
    public static class Result
    {
        public static IActionResult SuccessData<TData>(TData data, string? message = null)
              => new OkObjectResult(new BaseResponse<TData, object?>
              {
                  Data = data,
                  Message = message ?? string.Empty,
              });

        public static IActionResult SuccessAction(string message)
           => new OkObjectResult(new BaseResponse<object?, object?>
           {
               Data = null,
               Message = message,
           });

        public static IActionResult FailError<TError>(TError error, string message, int statusCode)
        {
            var payload = new BaseResponse<object?, TError>
            {
                Data = null,
                Error = error,
                Message = message
            };
            return new ObjectResult(payload) { StatusCode = statusCode };
        }

        public static IActionResult FailFieldErrors(
            IDictionary<string, string> fieldErrors,
            string message,
            int statusCode)
            => new ObjectResult(new BaseResponse<object?, IDictionary<string, string>>
            {
                Data = null,
                Error = new Dictionary<string, string>(fieldErrors),
                Message = message
            })
            { StatusCode = statusCode };

        public static IActionResult FailErrors(
            IList<string> errors,
            string message,
            int statusCode)
            => new ObjectResult(new BaseResponse<object?, IList<string>>
            {
                Data = null,
                Error = errors,
                Message = message
            })
            { StatusCode = statusCode };
    }
}

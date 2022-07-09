using Microsoft.AspNetCore.Http;

namespace ChallengeNet.Core.Models.Response
{
    public sealed class CoreResult<TResponse>
        where TResponse : class
    {
        public int HttpStatusCode { get; private set; }
        public TResponse Data { get; private set; }
        public string ErrorTitle { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool HasError { get; private set; }

        private static CoreResult<TResponse> AsError(int httpStatusCode, string errorTitle, string errorMessage)
        {
            return new CoreResult<TResponse>()
            {
                HttpStatusCode = httpStatusCode,
                HasError = true,
                ErrorTitle = errorTitle,
                ErrorMessage = errorMessage
            };
        }

        public static CoreResult<TResponse> AsOk(TResponse responseData)
        {
            return new CoreResult<TResponse>()
            {
                HttpStatusCode = StatusCodes.Status200OK,
                HasError = false,
                Data = responseData
            };
        }

        public static CoreResult<TResponse> AsNotFound(string errorMessage)
        {
            var statusCode = StatusCodes.Status404NotFound;
            
            return AsError(statusCode, "Not Found", errorMessage);
        }

        public static CoreResult<TResponse> AsBadRequest(string errorMessage)
        {
            var statusCode = StatusCodes.Status400BadRequest;
            
            return AsError(statusCode, "Bad Request", errorMessage);
        }

        public static CoreResult<TResponse> AsError(string errorMessage)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            
            return AsError(statusCode, "Internal server error", errorMessage);
        }
    }
}
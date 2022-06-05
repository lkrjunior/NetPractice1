using System;
using Microsoft.AspNetCore.Http;

namespace ChallengeNet.Core.Models.Response
{
    public sealed class CoreResult<TResponse>
        where TResponse : class
    {
        public int HttpStatusCode { get; set; }
        public TResponse Data { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool HasError { get; private set; }

        private static CoreResult<TResponse> AsError(int httpStatusCode, string errorMessage)
        {
            return new CoreResult<TResponse>()
            {
                HttpStatusCode = httpStatusCode,
                HasError = true,
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
            return AsError(StatusCodes.Status404NotFound, errorMessage);
        }

        public static CoreResult<TResponse> AsUnauthorized(string errorMessage)
        {
            return AsError(StatusCodes.Status401Unauthorized, errorMessage);
        }

        public static CoreResult<TResponse> AsBadRequest(string errorMessage)
        {
            return AsError(StatusCodes.Status400BadRequest, errorMessage);
        }

        public static CoreResult<TResponse> AsError(string errorMessage)
        {
            return AsError(StatusCodes.Status500InternalServerError, errorMessage);
        }
    }
}
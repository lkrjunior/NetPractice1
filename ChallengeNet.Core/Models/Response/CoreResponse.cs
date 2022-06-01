using System;
using Microsoft.AspNetCore.Http;

namespace ChallengeNet.Core.Models.Response
{
    public sealed class CoreResponse<TResponse>
        where TResponse : class
    {
        public int HttpStatusCode { get; set; }
        public TResponse Data { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool HasError { get; private set; }

        private static CoreResponse<TResponse> AsError(int httpStatusCode, string errorMessage)
        {
            return new CoreResponse<TResponse>()
            {
                HttpStatusCode = httpStatusCode,
                HasError = true,
                ErrorMessage = errorMessage
            };
        }

        public static CoreResponse<TResponse> AsOk(TResponse responseData)
        {
            return new CoreResponse<TResponse>()
            {
                HttpStatusCode = StatusCodes.Status200OK,
                HasError = false,
                Data = responseData
            };
        }

        public static CoreResponse<TResponse> AsNotFound(string errorMessage)
        {
            return AsError(StatusCodes.Status404NotFound, errorMessage);
        }

        public static CoreResponse<TResponse> AsUnauthorized(string errorMessage)
        {
            return AsError(StatusCodes.Status401Unauthorized, errorMessage);
        }

        public static CoreResponse<TResponse> AsBadRequest(string errorMessage)
        {
            return AsError(StatusCodes.Status400BadRequest, errorMessage);
        }

        public static CoreResponse<TResponse> AsError(string errorMessage)
        {
            return AsError(StatusCodes.Status500InternalServerError, errorMessage);
        }
    }
}
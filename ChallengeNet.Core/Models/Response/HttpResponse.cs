using System;
using Microsoft.AspNetCore.Http;

namespace ChallengeNet.Core.Models.Response
{
    public sealed class HttpResponse<T>
        where T : class
    {
        public int HttpStatusCode { get; set; }
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool HasError { get; private set; }

        private static HttpResponse<T> AsError(int httpStatusCode, string errorMessage)
        {
            return new HttpResponse<T>()
            {
                HttpStatusCode = httpStatusCode,
                HasError = true,
                ErrorMessage = errorMessage
            };
        }

        public static HttpResponse<T> AsOk(T response)
        {
            return new HttpResponse<T>()
            {
                HttpStatusCode = StatusCodes.Status200OK,
                HasError = false,
                Data = response
            };
        }

        public static HttpResponse<T> AsNotFound(string errorMessage)
        {
            return AsError(StatusCodes.Status404NotFound, errorMessage);
        }

        public static HttpResponse<T> AsUnauthorized(string errorMessage)
        {
            return AsError(StatusCodes.Status401Unauthorized, errorMessage);
        }

        public static HttpResponse<T> AsBadRequest(string errorMessage)
        {
            return AsError(StatusCodes.Status400BadRequest, errorMessage);
        }

        public static HttpResponse<T> AsError(string errorMessage)
        {
            return AsError(StatusCodes.Status500InternalServerError, errorMessage);
        }
    }
}
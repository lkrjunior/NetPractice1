using System;
using Microsoft.AspNetCore.Http;

namespace ChallengeNet.Core.Models.Response
{
    public sealed class HttpResponse
    {
        public int HttpStatusCode { get; set; }
        public object Data { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool HasError { get; private set; }

        private static HttpResponse AsError(int httpStatusCode, string errorMessage)
        {
            return new HttpResponse()
            {
                HttpStatusCode = httpStatusCode,
                HasError = true,
                ErrorMessage = errorMessage
            };
        }

        public static HttpResponse AsOk(object responseData)
        {
            return new HttpResponse()
            {
                HttpStatusCode = StatusCodes.Status200OK,
                HasError = false,
                Data = responseData
            };
        }

        public static HttpResponse AsNotFound(string errorMessage)
        {
            return AsError(StatusCodes.Status404NotFound, errorMessage);
        }

        public static HttpResponse AsUnauthorized(string errorMessage)
        {
            return AsError(StatusCodes.Status401Unauthorized, errorMessage);
        }

        public static HttpResponse AsBadRequest(string errorMessage)
        {
            return AsError(StatusCodes.Status400BadRequest, errorMessage);
        }

        public static HttpResponse AsError(string errorMessage)
        {
            return AsError(StatusCodes.Status500InternalServerError, errorMessage);
        }
    }
}
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeNet.Core.Models.Response
{
    public sealed class CoreResult<TResponse>
        where TResponse : class
    {
        public int HttpStatusCode { get; private set; }
        public TResponse Data { get; private set; }
        public ProblemDetails Error { get; private set; }
        public bool HasError { get; private set; }

        private static CoreResult<TResponse> AsError(int httpStatusCode, string title, string errorMessage)
        {
            return new CoreResult<TResponse>()
            {
                HttpStatusCode = httpStatusCode,
                HasError = true,
                Error = new ProblemDetails()
                {
                    Title = title,
                    Detail = errorMessage,
                    Status = httpStatusCode
                },
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
            var title = nameof(StatusCodes.Status404NotFound);

            return AsError(statusCode, title, errorMessage);
        }

        public static CoreResult<TResponse> AsUnauthorized(string errorMessage)
        {
            var statusCode = StatusCodes.Status401Unauthorized;
            var title = nameof(StatusCodes.Status401Unauthorized);

            return AsError(statusCode, title, errorMessage);
        }

        public static CoreResult<TResponse> AsBadRequest(string errorMessage)
        {
            var statusCode = StatusCodes.Status400BadRequest;
            var title = nameof(StatusCodes.Status400BadRequest);

            return AsError(statusCode, title, errorMessage);
        }

        public static CoreResult<TResponse> AsError(string errorMessage)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var title = nameof(StatusCodes.Status500InternalServerError);

            return AsError(statusCode, title, errorMessage);
        }
    }
}
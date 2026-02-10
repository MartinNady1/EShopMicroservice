using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ResultPattern
{
    public class Error
    {
        public string Message { get; }
        public HttpStatusCode Code { get; }
        public Dictionary<string, string>? ValidationErrors { get; }
        public Error(string message, HttpStatusCode code , Dictionary<string, string>? validationErrors = null)
        {
            Message = message;
            Code = code;
            ValidationErrors = validationErrors;
        }
        public static Error BadRequest(string message) =>
        new Error(message, HttpStatusCode.BadRequest);
        public static Error Validation(string message, Dictionary<string, string>? validationErrors = null)
            => new(message, HttpStatusCode.BadRequest, validationErrors);
        public static Error Unauthorized(string message = "Unauthorized") =>
            new Error(message, HttpStatusCode.Unauthorized);

        public static Error Forbidden(string message = "Forbidden") =>
            new Error(message, HttpStatusCode.Forbidden);

        public static Error NotFound(string message) =>
            new Error(message, HttpStatusCode.NotFound);

        public static Error Conflict(string message) =>
            new Error(message, HttpStatusCode.Conflict);

        public static Error InternalServerError(string message = "Internal server error") =>
            new Error(message, HttpStatusCode.InternalServerError);

        // Generic method if needed
        public static Error Custom(string message, HttpStatusCode code) =>
            new Error(message, code);

    }
}

using ExtraDry.Core;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ExtraDry.Server {

    /// <summary>
    /// Simple payload for all APIs that need to return information about a non 2xx status code.
    /// </summary>
    public static class ExceptionResponse {

        public static void RewriteResponse(HttpResponse response, HttpStatusCode code, string display, string description)
        {
            var er = new ErrorResponse {
                StatusCode = (int)code,
                Description = description,
                Display = display,

            };
            var body = JsonSerializer.Serialize(er);
            response.Clear();
            response.StatusCode = (int)code;
            response.ContentType = "application/json";
            response.ContentLength = body.Length;
            response.WriteAsync(body);
        }

    }
}

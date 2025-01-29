using ExtraDry.Core.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ExtraDry.Core.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task AssertSuccess(this HttpResponseMessage response, ILogger logger)
    {
        if(!response.IsSuccessStatusCode) {
            ProblemDetails? problem = null;
            try {
                problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                if(problem == null) {
                    var body = await response.Content.ReadAsStringAsync();
                    problem = new ProblemDetails {
                        Status = (int)response.StatusCode,
                        Title = DataConverter.CamelCaseToTitleCase(response.StatusCode.ToString()),
                        Detail = body,
                        Type = $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{(int)response.StatusCode}",
                        Instance = response.RequestMessage?.RequestUri?.AbsolutePath,
                    };
                }
                logger.LogError("HTTP request failed code {Status}, type {Type} at {Instance}; {Detail}", problem.Status, problem.Type, problem.Instance, problem.Detail);
            }
            catch {
                logger.LogError($"Attempt to extract problem details from request failed.");
            }
            throw new DryException(problem);
        }
    }
}

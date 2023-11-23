using ExtraDry.Core.Models;
using System.Net.Http.Json;

namespace ExtraDry.Blazor.Extensions;

public static class HttpResponseMessageExtensions {

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
                logger.LogProblemDetails(problem);
            }
            catch {
                logger.LogConsoleError($"Attempt to extract problem details from request failed.");
            }
            throw new DryException(problem);
        }
    }

}

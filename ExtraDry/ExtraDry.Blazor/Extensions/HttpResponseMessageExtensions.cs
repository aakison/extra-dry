#nullable enable

using ExtraDry.Core.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ExtraDry.Blazor.Extensions;

public static class HttpResponseMessageExtensions {

    public static async Task AssertSuccess(this HttpResponseMessage response, ILogger? logger = null)
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
            }
            catch {
                logger?.LogError($"Attempt to assert success failed to parse response.");
            }
            logger?.LogDebug("Response was not successful, full problem: {problem}", problem);
            throw new DryException(problem);
        }
    }

}

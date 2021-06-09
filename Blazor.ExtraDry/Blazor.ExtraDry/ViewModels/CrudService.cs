#nullable enable

using Blazor.ExtraDry.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {

    public class CrudService<T> {

        public CrudService(HttpClient client, string entityEndpointTemplate)
        {
            http = client;
            ApiTemplate = entityEndpointTemplate;
        }

        public string ApiTemplate { get; set; }

        public async Task CreateAsync(T item, params object[] args)
        {
            // TODO: Map DryException on server to DryException in Blazor for better messaging...
            var json = JsonSerializer.Serialize(item);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = ApiEndpoint("POST", args);
            try {
                var response = await http.PostAsync(endpoint, content);
                await AssertSuccess(response);
            }
            catch(Exception ex) {
                Console.Error.WriteLine($"Server Side Error while processing CrudService.CreateAsync.\n" +
                    $"API Endpoint: POST {endpoint}\n" +
                    $"Error Response: {ex.Message}");
                throw;
            }
        }

        public async Task<T?> RetrieveAsync(object key, params object[] args)
        {
            // TODO: Map DryException on server to DryException in Blazor for better messaging...
            var endpoint = ApiEndpoint("RetrieveAsync", key, args);
            Console.WriteLine(endpoint);
            var item = await http.GetFromJsonAsync<T>(endpoint);
            return item;
        }

        public async Task UpdateAsync(object key, T item, params object[] args)
        {
            // TODO: Map DryException on server to DryException in Blazor for better messaging...
            var endpoint = ApiEndpoint("UpdateAsync", key, args);
            Console.WriteLine(endpoint);
            try {
                var response = await http.PutAsJsonAsync(endpoint, item);
                await AssertSuccess(response);
            }
            catch(Exception ex) {
                Console.Error.WriteLine($" {ex.Message}");
            }
        }

        public async Task DeleteAsync(object key, params object[] args)
        {
            // TODO: Map DryException on server to DryException in Blazor for better messaging...
            var endpoint = ApiEndpoint("DeleteAsync", key, args);
            Console.WriteLine(endpoint);
            var response = await http.DeleteAsync(endpoint);
            await AssertSuccess(response);
        }

        private static async Task AssertSuccess(HttpResponseMessage response)
        {
            Console.WriteLine("Asserting success.");
            try {
                await response.EnsureSuccessStatusCodeAsync();
                // Handle success
            }
            catch(SimpleHttpResponseException exc) {
                // Handle failure
                var x2 = exc.Data;
            }

            if(!response.IsSuccessStatusCode) {
                var userMessage = "An unspecified error has occurred.";
                try {
                    // Map server side validation messages.
                    var str = await response.Content.ReadAsStringAsync();
                    var message = await response.Content.ReadFromJsonAsync<ErrorContent>();
                    if(message == null) {
                        userMessage = await response.Content.ReadAsStringAsync();
                    }
                    else {
                        var individualMessages = message.Errors.SelectMany(e => e.Value, (e, f) => f);
                        userMessage = string.Join("; ", individualMessages);
                    }
                }
                catch(Exception ex) {
                    // Just eat it.
                    var x = ex;
                }
                throw new DryException(response.ReasonPhrase, userMessage);
            }
        }

        private string ApiEndpoint(string method, object key, params object[] args)
        {
            try {
                var formatArgs = new List<object>(args);
                formatArgs.Insert(0, key);
                args = formatArgs.ToArray();
                return string.Format(ApiTemplate, args).TrimEnd('/');
            }
            catch(FormatException ex) {
                var argsFormatted = string.Join(',', args?.Select(e => e?.ToString()) ?? Array.Empty<string>());
                var message = $"Formatting problem while constructing endpoint for `CrudService.{method}`.  Typically the endpoint provided has additional placeholders that have not been provided. The endpoint template ({ApiTemplate}), could not be satisifed with arguments ({argsFormatted}).  Inner Exception was:  {ex.Message}";
                Console.Error.WriteLine(message);
                throw new DryException(message, "Error occurred connecting to server.");
            }
        }

        private readonly HttpClient http;

    }


    public static class HttpResponseMessageExtensions {
        public static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if(response.IsSuccessStatusCode) {
                return;
            }

            var content = await response.Content.ReadAsStringAsync();

            if(response.Content != null) {
                response.Content.Dispose();
            }

            var error = JsonSerializer.Deserialize<ErrorContent>(content);

            if(error == null) { 
                throw new SimpleHttpResponseException(response.StatusCode, content);
            }
            else {
                var message = $"Validation issues: {string.Join(',', error.Errors.Keys)}";
                throw new DryException(message);
            }
        }
    }

    public class SimpleHttpResponseException : Exception {
        public HttpStatusCode StatusCode { get; private set; }

        public SimpleHttpResponseException(HttpStatusCode statusCode, string content) : base(content)
        {
            StatusCode = statusCode;
        }
    }

}

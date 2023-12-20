using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Text.Json;

namespace ExtraDry.Swashbuckle.Tests.Filters
{
    public class SignatureImpliesStatusCodesTests
    {
        SchemaGenerator schemaRegistry;
        SchemaRepository schemaRepository;

        public SignatureImpliesStatusCodesTests()
        {
            schemaRegistry = new SchemaGenerator(new SchemaGeneratorOptions(), new JsonSerializerDataContractResolver(new JsonSerializerOptions()));
            schemaRepository = new SchemaRepository();
        }

        [Fact]
        public void CreateObject()
        {
            var filter = new SignatureImpliesStatusCodes(new ExtraDryGenOptions.HttpMethodsOptions());

            Assert.NotNull(filter);
        }

        [Theory]
        [MemberData(nameof(ChangeResponseCodeData))]
        public void ChangeResponseCode(HttpMethod httpMethod, HttpStatusCode newHttpStatusCode, string description, string testMethod, string expectedCode, string expectedDesc)
        {
            var options = new ExtraDryGenOptions.HttpMethodsOptions();
            options.AddMapping(httpMethod, newHttpStatusCode, description);
            var filter = new SignatureImpliesStatusCodes(options);
            var defaultResponse = new OpenApiResponse();
            defaultResponse.Content.Add("Example", new OpenApiMediaType());
            var operation = new OpenApiOperation();
            operation.Responses.Add("200", defaultResponse);
            var context = new OperationFilterContext(null, schemaRegistry, schemaRepository, typeof(DummyController).GetMethod(testMethod));

            filter.Apply(operation, context);

            Assert.False(operation.Responses.ContainsKey("200"));
            Assert.True(operation.Responses.ContainsKey(expectedCode));
            var newResponse = operation.Responses[expectedCode];
            Assert.NotNull(newResponse.Content);
            Assert.Equal(expectedDesc, newResponse.Description);
        }

        public static IEnumerable<object[]> ChangeResponseCodeData()
        {
            yield return new object[] { HttpMethod.Get,     HttpStatusCode.Accepted,        string.Empty,   nameof(DummyController.GetMethod),      "202", "Accepted" };
            yield return new object[] { HttpMethod.Get,     HttpStatusCode.Found,           "Success",      nameof(DummyController.GetMethod),      "302", "Success" };
            yield return new object[] { HttpMethod.Put,     HttpStatusCode.ResetContent,    string.Empty,   nameof(DummyController.PutMethod),      "205", "ResetContent" };
            yield return new object[] { HttpMethod.Put,     HttpStatusCode.MultiStatus,     "Success",      nameof(DummyController.PutMethod),      "207", "Success" };
            yield return new object[] { HttpMethod.Post,    HttpStatusCode.IMUsed,          string.Empty,   nameof(DummyController.PostMethod),     "226", "IMUsed" };
            yield return new object[] { HttpMethod.Post,    HttpStatusCode.NoContent,       "Success",      nameof(DummyController.PostMethod),     "204", "Success" };
            yield return new object[] { HttpMethod.Delete,  HttpStatusCode.Redirect,        string.Empty,   nameof(DummyController.DeleteMethod),   "302", "Success" };
            yield return new object[] { HttpMethod.Delete,  HttpStatusCode.Accepted,        "Completed",    nameof(DummyController.DeleteMethod),   "202", "Completed" };
        }
    }

    internal class DummyController
    {
        [HttpGet]
        public void GetMethod() { }

        [HttpPut]
        public void PutMethod() { }

        [HttpPost]
        public void PostMethod() { }

        [HttpDelete]
        public void DeleteMethod() { }
    }
}

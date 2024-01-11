using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
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
            var filter = new SignatureImpliesStatusCodes();

            Assert.NotNull(filter);
        }

        [Theory]
        [MemberData(nameof(DefaultResponseCodeData))]
        public void DefaultResponseCode(string testMethod, string expectedCode, string expectedDesc)
        {
            var filter = new SignatureImpliesStatusCodes();
            var defaultResponse = new OpenApiResponse();
            defaultResponse.Description = "Success";
            defaultResponse.Content.Add("Example", new OpenApiMediaType());
            var operation = new OpenApiOperation();
            operation.Responses.Add("200", defaultResponse);
            var context = new OperationFilterContext(null, schemaRegistry, schemaRepository, typeof(DummyController).GetMethod(testMethod));

            filter.Apply(operation, context);

            Assert.True(operation.Responses.ContainsKey(expectedCode));
            var newResponse = operation.Responses[expectedCode];
            Assert.NotNull(newResponse.Content);
            Assert.Equal(expectedDesc, newResponse.Description);
        }

        public static IEnumerable<object[]> DefaultResponseCodeData()
        {
            yield return new object[] { nameof(DummyController.GetMethod), "200", "Success" };
            yield return new object[] { nameof(DummyController.GetMethodWithResponsePayload), "200", "Success" };
            yield return new object[] { nameof(DummyController.PutMethod), "200", "Success" };
            yield return new object[] { nameof(DummyController.PutMethodWithResponsePayload), "200", "Success" };
            yield return new object[] { nameof(DummyController.PostMethod), "201", "Created" };
            yield return new object[] { nameof(DummyController.PostMethodWithResponsePayload), "201", "Created" };
            yield return new object[] { nameof(DummyController.DeleteMethod), "204", "Success" };
            yield return new object[] { nameof(DummyController.DeleteMethod), "204", "Success" };
        }
    }

    internal class DummyController
    {
        [HttpGet]
        public void GetMethod() { }

        [HttpGet]
        public object GetMethodWithResponsePayload() => new();

        [HttpPut]
        public void PutMethod() { }

        [HttpPut]
        public object PutMethodWithResponsePayload() => new();

        [HttpPost]
        public void PostMethod() { }

        [HttpPost]
        public object PostMethodWithResponsePayload() => new();

        [HttpDelete]
        public void DeleteMethod() { }
    }
}

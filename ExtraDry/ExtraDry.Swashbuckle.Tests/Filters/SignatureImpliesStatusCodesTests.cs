using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ExtraDry.Swashbuckle.Tests.Filters;

public class SignatureImpliesStatusCodesTests
{
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
    [InlineData(nameof(DummyController.GetMethod), "200", "Success")]
    [InlineData(nameof(DummyController.GetMethodWithResponsePayload), "200", "Success")]
    [InlineData(nameof(DummyController.PutMethod), "200", "Success")]
    [InlineData(nameof(DummyController.PutMethodWithResponsePayload), "200", "Success")]
    [InlineData(nameof(DummyController.PostMethod), "201", "Created")]
    [InlineData(nameof(DummyController.PostMethodWithResponsePayload), "201", "Created")]
    [InlineData(nameof(DummyController.DeleteMethod), "204", "Success")]
    public void DefaultResponseCode(string testMethod, string expectedCode, string expectedDesc)
    {
        var filter = new SignatureImpliesStatusCodes();
        var defaultResponse = new OpenApiResponse {
            Description = "Success"
        };
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

    private readonly SchemaGenerator schemaRegistry;

    private readonly SchemaRepository schemaRepository;
}

[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Align with real controller")]
internal class DummyController
{
    [HttpGet]
    public void GetMethod()
    { }

    [HttpGet]
    public object GetMethodWithResponsePayload() => new();

    [HttpPut]
    public void PutMethod()
    { }

    [HttpPut]
    public object PutMethodWithResponsePayload() => new();

    [HttpPost]
    public void PostMethod()
    { }

    [HttpPost]
    public object PostMethodWithResponsePayload() => new();

    [HttpDelete]
    public void DeleteMethod()
    { }
}

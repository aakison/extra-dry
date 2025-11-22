## Sample Components API

The Components API is a microservice responsible for common cross-cutting functional patterns for many different types of components, typically hosted in other microservices.  Central to this is Attachments and Conversations, but also includes Tags and other common features.  

This is designed to be used for a SaaS system, with many different tenants supported.

### Endpoints Summary


### Architecture

### Controllers and Services

The API is designed to be thin, with standard ASP.NET MVC controllers and services in the respectively named directories.  Additionally, common bussines rules, querying, paging and more for services is provided by Extra Dry.

### Data Storage

This microservice is designed to handle massive amounts of data, for 10 clients with 10 entities, each with 10 entries, this will store 1000 components.  For 100 clients, with 20 entities, averaging 10,000 entries each, this grows to 20,000,000 components.

For this large amount of data, the globally distributed NoSQL database, Azure CosmosDB, is used.  Entity Framework Core is used to abstract the implementation, and Docker is used to run the database locally for development.

The implementation is fairly short, with only half a dozen entities in the EF Database Context.  The models themselves are defined in the [Sample Components Shared](..\..\Sample.Components.Shared\README.md) project.  

The entities should support the `ITenanted` interface and the Extra Dry `IResourceIdentifiers` interface.

### Security

#### RBAC Authorization
[RBAC](https://en.wikipedia.org/wiki/Role-based_access_control) is a simple way to manage access to resources.  It is relatively simple to implement and is the first line of authorization for all endpoints.

#### ABAC Authorization
[ABAC](https://en.wikipedia.org/wiki/Attribute-based_access_control) is a more flexible and powerful way to manage access to resources. Attributes are stored with Components and ABAC rules are evaluated against user claims.

See [Security README](./Security/README.md) for more details.

### Configuration

The API uses the built-in .NET Core configuration system, which chains a `appsettings.json`, `launchSettings.json`, environment variables, user secrets and more.  

The `Options` directory has a nested model for all options, which also provides validation rules to facilitate configuration of CD.  *Remember not to store secrets in the configuration files, use user-secrets instead*.

The options can be dependecy injected into any class using `ApiOptions` (as hot reloading is not allowed in CD scenarios, `IOptions` is not used).

Logging of options is the first thing done when the host is started using the Extra Dry properties loggin extensions.  Decorate options that contain secrets with the `Secret` attribute to prevent them from being logged by the extensions (this is a very lightweight version of the redaction mechanism for dotnet).

### Health Checks

This API has a health check endpoint at `/healthcheck`.  This uses the built-in .NET Core health check system.  A custom health check is implemented in the `ApiHealthCheck` class.  Additionally, the health checks are configured in Program.cs to use both this and the built-in CosmosDB health check.

### Debugging

There are a set of .http files in the `HttpTests` folder which provide manual testing scenarios to use during development and debugging.  These provide a fast way to test the API without needing to write a client or use the Postman.  Additionally, these automatically use user secrets for the `Authorization` header of requests.

See [HttpTests README](./HttpTests/README.md) for more details, including some Powershell snippets to configure the JWT tokens in user secrets.


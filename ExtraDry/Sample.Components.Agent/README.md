# Sample.Components.Agent

The `Agent` component is responsible for event processing for the `Components` microservice. 

A `Component` is any entity that is embellished with the following features:
  * Attachments
  * Conversations
  * Tags

## Dependencies

The `Agent` depends on the following:
  * `Sample.Components.Api` - The `Agent` component uses the `API` component to communicate with the `Components` microservice.
  * A service bus, to consume events about updated `Components` or `Tenants`, which is one of:
    * InMemory - A transient bus that is suitable for unit testing.
    * RabbitMQ - A durable bus suitable for development and testing.
    * Azure Service Bus - A durable bus suitable for production.

## Health Checks

The `Agent` component exposes a health check at the `/healthcheck` endpoint.  As such, it is 
technically an ASP.NET MVC application.  However, this is the only exposed endpoint.

See: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0

## Configuration

To avoid collision with other processes during debugging, the preferred IP addresses, domain names 
and ports are:
- Docker IP address: 172.20.0.41 (components-agent)
  - HTTP port:  80
  - HTTPS port: 443
- Standalone IP address: 127.0.0.1 (localhost)
  - HTTP port:  41080
  - HTTPS port: 41443

To configure these properly, the following files and settings are used:
- `./launchsettings.json`
- `./Dockerfile`
- `../docker-compose.yml`

### Docker Compose

The `Agent` component can be run locally using Docker Compose.  The `docker-compose.yml` file in 
the root of the repository contains the necessary configuration to run the `Agent` component 
locally.


### Notes

For issues with SSL certificates for development Docker instances, see all required steps at: 

see https://github.com/dotnet/AspNetCore.Docs/issues/6199

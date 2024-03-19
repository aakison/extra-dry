### Security

This describes the security for the [Sample Components API](..\README.md) application.

#### JWT Bearer Tokens

All access to the APIs is done exclusively through a JWT Bearer Token.  RBAC rules are evaluated against 'roles' defined in the JWT, while ABAC rules may include other information in the token, such as 'sub' or any 'claims' provided.

#### RBAC

There are only three distinct roles in the application, **user**, **admin** and **agent**.  RBAC security is applied through policies defined on each endpoint.  

**user**
: This basic role is permitted to create, retrieve, update and delete `Attachments`, `Conversations` and `Tags`.  `Components` are read-only for this role, and no access to `Tenants` is allowed.  ABAC rules are then applied, which are typically much more restrictive. 

**admin**
: This advanced role has full access to `Tenants` for maintenance of the system.  No access to `Attachments`, `Conversations` or `Tags` is defined for this role; however the role is typically issued with the **user** role.

**agent**
: The role is designed to be used by the [Sample Components Agent](..\Sample.Components.Agent\README.md).  It has the ability to create, retrieve, update and delete `Components`, which are just facades for entities in other microservices, referenced by UUID.  Valid `Components` are determined by events that are published by other systems.

#### ABAC

TODO: This is not yet implemented.  

#### Implementation

The [Policies.cs](.\Policies.cs) file contains the names of, for use in `Authorize` attributes on contoller endpoints.  E.g. `[Authorize(Policies.Agent)]`.  In addition the roles defined above, there are composite roles, such as `Policies.AdminOrAgent` which is used to allow access to endpoints for both `admin` and `agent` roles.

The [Progam.cs](..\Startup.cs) file contains the configuration for the security middleware.  This includes the JWT token validation, and the policy configuration.  The `AddAuthorization` method is used to add the policies to the service collection, and the `AddAuthentication` method is used to add the JWT token validation.

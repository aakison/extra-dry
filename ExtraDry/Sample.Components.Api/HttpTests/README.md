## HTTP Tests for basic correctness testing during development

API Calls are described in the `Sample.Components.Api.http` file.  

Common settings for calls are in the `http-client.env.json` file, with secure settings being stored
in .NET User Secrets.  

See https://learn.microsoft.com/en-us/aspnet/core/test/http-files?view=aspnetcore-8.0

### Configuring Tokens for Development

To use user-secrets, you need to have the `dotnet user-secrets` tool installed.  Then, a 
variable needs to be defined in the .json file with a link to the user-secret by name.  Finally,
the user-secret needs to be set.  For token based secrets, the token is created by 
`dotnet user-jwts`.

The following script will set up the user-secret for the different tokens required for testing, 
(run it from the project directory so user-jwts have the correct audience `aud`):

```PowerShell
$output = dotnet user-jwts create --role user
$token = $output.Split(' ')[-1]
dotnet user-secrets set "Tokens:UserRole" $token

$output = dotnet user-jwts create --role admin
$token = $output.Split(' ')[-1]
dotnet user-secrets set "Tokens:AdminRole" $token

$output = dotnet user-jwts create --role agent
$token = $output.Split(' ')[-1]
dotnet user-secrets set "Tokens:AgentRole" $token
```


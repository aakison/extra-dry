rmdir ./TestCoverage -Recurse
rmdir ./ExtraDry.Blazor.Tests/TestResults -Recurse
rmdir ./ExtraDry.Core.Tests/TestResults -Recurse
rmdir ./ExtraDry.Server.Tests/TestResults -Recurse
rmdir ./ExtraDry.Swashbuckle.Tests/TestResults -Recurse
rmdir ./Sample.Tests/TestResults -Recurse

dotnet test ExtraDry.sln --collect:"XPlat Code Coverage"
reportgenerator -reports:./*/TestResults/*/*.xml -targetdir:./TestCoverage
./TestCoverage/index.htm

rmdir ./ExtraDry.Blazor.Tests/TestResults -Recurse
rmdir ./ExtraDry.Core.Tests/TestResults -Recurse
rmdir ./ExtraDry.Server.Tests/TestResults -Recurse
rmdir ./ExtraDry.Swashbuckle.Tests/TestResults -Recurse
rmdir ./Sample.Tests/TestResults -Recurse

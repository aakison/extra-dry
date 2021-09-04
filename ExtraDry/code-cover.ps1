rmdir ./TestCoverage -Recurse
rmdir ./ExtraDry.Blazor.Tests/TestResults -Recurse
rmdir ./ExtraDry.Core.Tests/TestResults -Recurse
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:./*/TestResults/*/*.xml -targetdir:./TestCoverage
./TestCoverage/index.htm

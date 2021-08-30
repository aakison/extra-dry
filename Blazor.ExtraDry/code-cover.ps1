rmdir ./TestCoverage -Recurse
rmdir ./Blazor.ExtraDry.Tests/TestResults -Recurse
rmdir ./Blazor.ExtraDry.Core.Tests/TestResults -Recurse
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:./*/TestResults/*/*.xml -targetdir:./TestCoverage
./TestCoverage/index.htm

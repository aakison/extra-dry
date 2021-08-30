dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:./*/TestResults/*/*.xml -targetdir:./TestCoverage
./TestCoverage/index.htm

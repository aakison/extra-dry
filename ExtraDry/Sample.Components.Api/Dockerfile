#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
RUN chmod a+rwx /usr/local/share/ca-certificates
RUN chmod a+rwx /etc/ssl/certs
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Sample.Components.Api/Sample.Components.Api.csproj", "Sample.Components.Api/"]
RUN dotnet restore "./Sample.Components.Api/./Sample.Components.Api.csproj"
COPY . .
WORKDIR "/src/Sample.Components.Api"
RUN dotnet build "./Sample.Components.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sample.Components.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/bin/bash", "-c", "./startup.sh"]
#ENTRYPOINT [ "dotnet", "Sample.Components.Api.dll" ]

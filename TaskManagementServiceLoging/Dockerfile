FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TaskManagementServiceLoging/TaskManagementServiceLoging.WebApi.csproj", "src/TaskManagementServiceLoging/"]
COPY ["src/TaskManagementServiceLoging.Application/TaskManagementServiceLoging.Application.csproj", "src/TaskManagementServiceLoging.Application/"]
COPY ["src/TaskManagementServiceLoging.Domain/TaskManagementServiceLoging.Domain.csproj", "src/TaskManagementServiceLoging.Domain/"]
COPY ["src/TaskManagementServiceLoging.Infrastructure/TaskManagementServiceLoging.Infrastructure.csproj", "src/TaskManagementServiceLoging.Infrastructure/"]

RUN dotnet restore "src/TaskManagementServiceLoging/TaskManagementServiceLoging.WebApi.csproj"

COPY src/ ./src/

RUN dotnet build "src/TaskManagementServiceLoging/TaskManagementServiceLoging.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "src/TaskManagementServiceLoging/TaskManagementServiceLoging.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TaskManagementServiceLoging.WebApi.dll"]

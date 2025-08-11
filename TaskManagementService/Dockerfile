FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TaskManagementService/TaskManagementService.WebApi.csproj", "src/TaskManagementService/"]
COPY ["src/TaskManagementService.Application/TaskManagementService.Application.csproj", "src/TaskManagementService.Application/"]
COPY ["src/TaskManagementService.Domain/TaskManagementService.Domain.csproj", "src/TaskManagementService.Domain/"]
COPY ["src/TaskManagementService.Persistence/TaskManagementService.Persistence.csproj", "src/TaskManagementService.Persistence/"]
COPY ["src/TaskManagementService.Messaging/TaskManagementService.Messaging.csproj", "src/TaskManagementService.Messaging/"]
COPY ["src/TaskManagementService.Identity/TaskManagementService.Identity.csproj", "src/TaskManagementService.Identity/"]
COPY ["src/TaskManagementService.Resources/TaskManagementService.Resources.csproj", "src/TaskManagementService.Resources/"]
RUN dotnet restore "src/TaskManagementService/TaskManagementService.WebApi.csproj"
COPY src/ ./src/
RUN dotnet build "src/TaskManagementService/TaskManagementService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/TaskManagementService/TaskManagementService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TaskManagementService.WebApi.dll"]

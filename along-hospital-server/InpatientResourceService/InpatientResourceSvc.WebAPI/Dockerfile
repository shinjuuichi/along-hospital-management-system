# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InpatientResourceService/InpatientResourceSvc.WebAPI/InpatientResourceSvc.WebAPI.csproj", "InpatientResourceService/InpatientResourceSvc.WebAPI/"]
COPY ["along-hospital-server.ServiceDefaults/along-hospital-server.ServiceDefaults.csproj", "along-hospital-server.ServiceDefaults/"]
COPY ["InpatientResourceService/InpatientResourceSvc.BLL/InpatientResourceSvc.BLL.csproj", "InpatientResourceService/InpatientResourceSvc.BLL/"]
COPY ["MessageBroker/MessageBroker.csproj", "MessageBroker/"]
COPY ["InpatientResourceService/InpatientResourceSvc.DAL/InpatientResourceSvc.DAL.csproj", "InpatientResourceService/InpatientResourceSvc.DAL/"]
COPY ["SharedLibrary/SharedLibrary.csproj", "SharedLibrary/"]
RUN dotnet restore "./InpatientResourceService/InpatientResourceSvc.WebAPI/InpatientResourceSvc.WebAPI.csproj"
COPY . .
WORKDIR "/src/InpatientResourceService/InpatientResourceSvc.WebAPI"
RUN dotnet build "./InpatientResourceSvc.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InpatientResourceSvc.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InpatientResourceSvc.WebAPI.dll"]
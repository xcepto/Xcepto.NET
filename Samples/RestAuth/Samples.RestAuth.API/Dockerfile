FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Samples.RestAuth.API/Samples.RestAuth.API.csproj", "Samples.RestAuth.API/"]
RUN dotnet restore "Samples.RestAuth.API/Samples.RestAuth.API.csproj"
COPY . .
WORKDIR "/src/Samples.RestAuth.API"
RUN dotnet build "Samples.RestAuth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Samples.RestAuth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Samples.RestAuth.API.dll"]

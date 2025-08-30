FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Samples.Shopping.Api/Samples.Shopping.Api.csproj", "Samples.Shopping.Api/"]
RUN dotnet restore "Samples.Shopping.Api/Samples.Shopping.Api.csproj"
COPY . .
WORKDIR "/src/Samples.Shopping.Api"
RUN dotnet build "Samples.Shopping.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Samples.Shopping.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Samples.Shopping.Api.dll"]

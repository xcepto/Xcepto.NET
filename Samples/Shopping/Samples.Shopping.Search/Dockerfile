FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Samples.Shopping.Search/Samples.Shopping.Search.csproj", "Samples.Shopping.Search/"]
RUN dotnet restore "Samples.Shopping.Search/Samples.Shopping.Search.csproj"
COPY . .
WORKDIR "/src/Samples.Shopping.Search"
RUN dotnet build "Samples.Shopping.Search.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Samples.Shopping.Search.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Samples.Shopping.Search.dll"]

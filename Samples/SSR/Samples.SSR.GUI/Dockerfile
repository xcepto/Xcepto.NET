FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Samples.SSR.GUI/Samples.SSR.GUI.csproj", "Samples.SSR.GUI/"]
RUN dotnet restore "Samples.SSR.GUI/Samples.SSR.GUI.csproj"
COPY . .
WORKDIR "/src/Samples.SSR.GUI"
RUN dotnet build "Samples.SSR.GUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Samples.SSR.GUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Samples.SSR.GUI.dll"]

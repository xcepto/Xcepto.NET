FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Samples.Shopping.Cart/Samples.Shopping.Cart.csproj", "Samples.Shopping.Cart/"]
RUN dotnet restore "Samples.Shopping.Cart/Samples.Shopping.Cart.csproj"
COPY . .
WORKDIR "/src/Samples.Shopping.Cart"
RUN dotnet build "Samples.Shopping.Cart.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Samples.Shopping.Cart.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Samples.Shopping.Cart.dll"]

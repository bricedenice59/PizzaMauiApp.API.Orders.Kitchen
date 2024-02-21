FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PizzaMaui.API.Orders.Kitchen/PizzaMaui.API.Orders.Kitchen.csproj", "PizzaMaui.API.Orders.Kitchen/"]
RUN dotnet restore "PizzaMaui.API.Orders.Kitchen/PizzaMaui.API.Orders.Kitchen.csproj"
COPY . .
WORKDIR "/src/PizzaMaui.API.Orders.Kitchen"
RUN dotnet build "PizzaMaui.API.Orders.Kitchen.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PizzaMaui.API.Orders.Kitchen.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PizzaMaui.API.Orders.Kitchen.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/Orders/Orders.API/Orders.API.csproj", "src/services/Orders/Orders.API/"]
COPY ["src/services/Orders/Orders.BLL/Orders.BLL.csproj", "src/services/Orders/Orders.BLL/"]
COPY ["src/services/Orders/Orders.DAL/Orders.DAL.csproj", "src/services/Orders/Orders.DAL/"]
COPY ["src/services/Orders/Orders.Domain/Orders.Domain.csproj", "src/services/Orders/Orders.Domain/"]
COPY ["src/shared/Shared/Shared.csproj", "src/shared/Shared/"]

RUN dotnet restore "src/services/Orders/Orders.API/Orders.API.csproj"
COPY . .
WORKDIR "/src/src/services/Orders/Orders.API"
RUN dotnet build "./Orders.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Orders.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.API.dll"]
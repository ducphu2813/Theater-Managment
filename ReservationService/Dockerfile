# sử dụng image mcr.microsoft.com/dotnet/aspnet:8.0 để build project
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# sử dụng image mcr.microsoft.com/dotnet/sdk:8.0 để build project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReservationService/ReservationService.csproj", "ReservationService/"]
RUN dotnet restore "ReservationService/ReservationService.csproj"
COPY . .
WORKDIR "/src/ReservationService"
RUN dotnet build "ReservationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# publish project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ReservationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReservationService.dll"]

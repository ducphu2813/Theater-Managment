FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# build project Shared 1 lần và có thể cho các project khác sử dụng lại khi build docker
COPY ["../../Shared/Shared.csproj", "../Shared/"]
RUN dotnet restore "../Shared/Shared.csproj"
COPY Shared/. ./Shared/
RUN dotnet build "Shared/Shared.csproj" -c $BUILD_CONFIGURATION -o /app/shared

# copy file csproj vào thư mục làm việc và restore
COPY ["Reservation/Reservation.API/Reservation.API.csproj", "Reservation/Reservation.API/"]
COPY ["Reservation/Reservation.Application/Reservation.Application.csproj", "Reservation/Reservation.Application/"]
COPY ["Reservation/Reservation.Domain/Reservation.Domain.csproj", "Reservation/Reservation.Domain/"]
COPY ["Reservation/Reservation.Infrastructure/Reservation.Infrastructure.csproj", "Reservation/Reservation.Infrastructure/"]

RUN dotnet restore "Reservation/Reservation.API/Reservation.API.csproj"
COPY . .
WORKDIR "/src/Reservation/Reservation.API"
RUN dotnet build "Reservation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Reservation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reservation.API.dll"]

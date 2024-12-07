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
COPY ["Auth/Auth.API/Auth.API.csproj", "Auth/Auth.API/"]
COPY ["Auth/Auth.Application/Auth.Application.csproj", "Auth/Auth.Application/"]
COPY ["Auth/Auth.Domain/Auth.Domain.csproj", "Auth/Auth.Domain/"]
COPY ["Auth/Auth.Infrastructure/Auth.Infrastructure.csproj", "Auth/Auth.Infrastructure/"]

RUN dotnet restore "Auth/Auth.API/Auth.API.csproj"

COPY . .
WORKDIR "/src/Auth/Auth.API"
RUN dotnet build "Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]

# Stage 1: build base image cho runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# thư mục làm việc
WORKDIR /src

# build project Shared 1 lần và có thể cho các project khác sử dụng lại khi build docker
COPY ["../../Shared/Shared.csproj", "../Shared/"]
RUN dotnet restore "../Shared/Shared.csproj"
COPY Shared/. ./Shared/
RUN dotnet build "Shared/Shared.csproj" -c $BUILD_CONFIGURATION -o /app/shared


# copy file csproj vào thư mục làm việc và restore
COPY ["Movie/Movie.API/Movie.API.csproj", "Movie/Movie.API/"]
COPY ["Movie/Movie.Application/Movie.Application.csproj", "Movie/Movie.Application/"]
COPY ["Movie/Movie.Domain/Movie.Domain.csproj", "Movie/Movie.Domain/"]
COPY ["Movie/Movie.Infrastructure/Movie.Infrastructure.csproj", "Movie/Movie.Infrastructure/"]

RUN dotnet restore "Movie/Movie.API/Movie.API.csproj"
COPY . .
WORKDIR "/src/Movie/Movie.API"
RUN dotnet build "Movie.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Movie.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movie.API.dll"]

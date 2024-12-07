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
RUN dotnet build "../Shared/Shared.csproj" -c $BUILD_CONFIGURATION -o /app/shared

# copy file csproj vào thư mục làm việc và restore
COPY ["Analysis/Analysis.API/Analysis.API.csproj", "Analysis/Analysis.API/"]
COPY ["Analysis/Analysis.Application/Analysis.Application.csproj", "Analysis/Analysis.Application/"]
COPY ["Analysis/Analysis.Domain/Analysis.Domain.csproj", "Analysis/Analysis.Domain/"]
COPY ["Analysis/Analysis.Infrastructure/Analysis.Infrastructure.csproj", "Analysis/Analysis.Infrastructure/"]

RUN dotnet restore "Analysis/Analysis.API/Analysis.API.csproj"
COPY . .
WORKDIR "/src/Analysis/Analysis.API"
RUN dotnet build "Analysis.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Analysis.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Analysis.API.dll"]

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
COPY ["Payment/Payment.API/Payment.API.csproj", "Payment/Payment.API/"]
COPY ["Payment/Payment.Application/Payment.Application.csproj", "Payment/Payment.Application/"]
COPY ["Payment/Payment.Domain/Payment.Domain.csproj", "Payment/Payment.Domain/"]
COPY ["Payment/Payment.Infrastructure/Payment.Infrastructure.csproj", "Payment/Payment.Infrastructure/"]

RUN dotnet restore "Payment/Payment.API/Payment.API.csproj"
COPY . .
WORKDIR "/src/Payment/Payment.API"
RUN dotnet build "Payment.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Payment.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.API.dll"]

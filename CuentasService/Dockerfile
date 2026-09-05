# ---- Etapa de compilación ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CuentasService/src/CuentasService.Domain/*.csproj CuentasService/src/CuentasService.Domain/
COPY CuentasService/src/CuentasService.Application/*.csproj CuentasService/src/CuentasService.Application/
COPY CuentasService/src/CuentasService.Infrastructure/*.csproj CuentasService/src/CuentasService.Infrastructure/
COPY CuentasService/src/CuentasService.API/*.csproj CuentasService/src/CuentasService.API/
COPY Shared/Contracts/*.csproj Shared/Contracts/

RUN dotnet restore CuentasService/src/CuentasService.API/CuentasService.API.csproj

COPY CuentasService/src/ CuentasService/src/
COPY Shared/Contracts/ Shared/Contracts/

WORKDIR /src/CuentasService/src/CuentasService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---- Etapa de ejecución ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "CuentasService.API.dll"]

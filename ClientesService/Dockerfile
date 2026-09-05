# ---- Etapa de compilación ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ClientesService/src/ClientesService.Domain/*.csproj ClientesService/src/ClientesService.Domain/
COPY ClientesService/src/ClientesService.Application/*.csproj ClientesService/src/ClientesService.Application/
COPY ClientesService/src/ClientesService.Infrastructure/*.csproj ClientesService/src/ClientesService.Infrastructure/
COPY ClientesService/src/ClientesService.API/*.csproj ClientesService/src/ClientesService.API/
COPY Shared/Contracts/*.csproj Shared/Contracts/

RUN dotnet restore ClientesService/src/ClientesService.API/ClientesService.API.csproj

COPY ClientesService/src/ ClientesService/src/
COPY Shared/Contracts/ Shared/Contracts/

WORKDIR /src/ClientesService/src/ClientesService.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---- Etapa de ejecución ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ClientesService.API.dll"]

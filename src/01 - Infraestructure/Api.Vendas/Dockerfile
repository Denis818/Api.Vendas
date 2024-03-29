#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/01 - Infraestructure/Api.Vendas/Api.Vendas.csproj", "src/01 - Infraestructure/Api.Vendas/"]
COPY ["src/02 - Application/Application/Application.csproj", "src/02 - Application/Application/"]
COPY ["src/01 - Infraestructure/Data/Data.csproj", "src/01 - Infraestructure/Data/"]
COPY ["src/03 - Domain/Domain/Domain.csproj", "src/03 - Domain/Domain/"]
RUN dotnet restore "./src/01 - Infraestructure/Api.Vendas/Api.Vendas.csproj"
COPY . .
WORKDIR "/src/src/01 - Infraestructure/Api.Vendas"
RUN dotnet build "./Api.Vendas.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.Vendas.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Vendas.dll"]
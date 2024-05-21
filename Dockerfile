#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["F-LocalBrand.csproj", "."]
RUN dotnet restore "./F-LocalBrand.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "F-LocalBrand.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "F-LocalBrand.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "F-LocalBrand.dll"]
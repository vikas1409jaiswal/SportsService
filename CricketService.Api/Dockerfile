# Dockerfile for CricketService.Api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files for restore
COPY ["CricketService.Api/CricketService.Api.csproj", "CricketService.Api/"]
COPY ["CricketService.Data/CricketService.Data.csproj", "CricketService.Data/"]
COPY ["CricketService.Domain/CricketService.Domain.csproj", "CricketService.Domain/"]
COPY ["CricketService.Hangfire/CricketService.Hangfire.csproj", "CricketService.Hangfire/"]
COPY ["CricketService.Utils/CricketService.Utils.csproj", "CricketService.Utils/"]

# Restore dependencies
RUN dotnet restore "CricketService.Api/CricketService.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/CricketService.Api"
RUN dotnet build "CricketService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CricketService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5001
ENTRYPOINT ["dotnet", "CricketService.Api.dll"]

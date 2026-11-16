# How Render builds and runs Bunker Survival.

# 1) Build: compile the backend and gather the game's pages into /app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY BunkerSurvival.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish BunkerSurvival.csproj -c Release -o /app --no-restore

# 2) Run: a small image with just the .NET runtime and the built game
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
# Render tells the app which port to use through PORT (logic.cs reads it)
ENTRYPOINT ["dotnet", "BunkerSurvival.dll"]

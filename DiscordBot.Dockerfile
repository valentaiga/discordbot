FROM mcr.microsoft.com/dotnet/sdk as build
WORKDIR /
COPY ["src/DiscordBot/DiscordBot.csproj", "src/DiscordBot/"]
COPY ["src/DiscordBot.Application/DiscordBot.Application.csproj", "src/DiscordBot.Application/"]
COPY ["src/DiscordBot.Domain/DiscordBot.Domain.csproj", "src/DiscordBot.Domain/"]
COPY ["src/DiscordBot.Infrastructure/DiscordBot.Infrastructure.csproj", "src/DiscordBot.Infrastructure/"]

COPY . .
WORKDIR src/DiscordBot
RUN dotnet publish DiscordBot.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "DiscordBot.dll"]
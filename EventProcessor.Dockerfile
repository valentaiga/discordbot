FROM mcr.microsoft.com/dotnet/sdk as build
WORKDIR /
COPY ["src/EventProcessor/EventProcessor.csproj", "src/DiscordBot/"]
COPY ["src/DiscordBot.Domain/DiscordBot.Domain.csproj", "src/DiscordBot.Domain/"]
COPY ["src/DiscordBot.Infrastructure/DiscordBot.Infrastructure.csproj", "src/DiscordBot.Infrastructure/"]

COPY . .
WORKDIR src/EventProcessor
RUN dotnet publish EventProcessor.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "EventProcessor.dll"]
FROM mcr.microsoft.com/dotnet/sdk as build
WORKDIR /
COPY ["src/TimedActionsWorker/TimedActionsWorker.csproj", "src/DiscordBot/"]
COPY ["src/DiscordBot.Infrastructure/DiscordBot.Infrastructure.csproj", "src/DiscordBot.Infrastructure/"]

COPY . .
WORKDIR src/TimedActionsWorker
RUN dotnet publish TimedActionsWorker.csproj -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "TimedActionsWorker.dll"]
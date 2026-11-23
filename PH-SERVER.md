# PH-Server fork of Requestrr

Branch `ph-server`, based on upstream tag `V2.1.10` (thomst08/requestrr).

## Changes

- `/request movie` and `/request tv` (and music) searches are always ephemeral: only the requester
  sees them and can drop them with Discord's "Dismiss message".
- When a request is sent, the confirmation card (embed + green "Request sent successfully" button
  + "Requested by @user") is posted publicly in the channel and the private message is deleted.
  If posting fails, it falls back to upstream behavior (editing the private message).
- `/issue` commands are unchanged.

Files: `RequestrrBot/ChatClients/Discord/DiscordPublicRequestMessage.cs` (new),
`DiscordMovieUserInterface.cs`, `DiscordTvShowUserInterface.cs`, `SlashCommands.txt`.

## Build and deploy

```
docker build -t requestrr-phserver:latest Requestrr.WebApi
docker compose -f Plex/docker-compose.tools.yml up -d requestrr
```

## Updating from upstream

```
git fetch upstream --tags
git rebase <new-tag>      # e.g. V2.1.11
```

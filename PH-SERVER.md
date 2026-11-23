# PH-Server fork of Requestrr

Branch `ph-server`, based on upstream tag `V2.1.10` (thomst08/requestrr).

## Changes

New Discord setting **PublicRequestConfirmations** (web UI → Chat Clients → Discord
miscellaneous settings, "Keep request searches private and post the confirmation publicly...";
`ChatClients.Discord.PublicRequestConfirmations` in settings.json, defaults to true if missing).
When on:

- `/request movie` and `/request tv` (and music) searches are ephemeral: only the requester
  sees them and can drop them with Discord's "Dismiss message".
- When a request is sent, the confirmation card (embed + green "Request sent successfully" button
  + "Requested by @user") is posted publicly in the channel and the private message is deleted.
  If posting fails, it falls back to upstream behavior (editing the private message).
- `/issue` commands are unchanged (still follow "Hide slash command request messages").

When off, behavior is upstream's.

Files: `RequestrrBot/ChatClients/Discord/DiscordPublicRequestMessage.cs` (new),
`DiscordMovieUserInterface.cs`, `DiscordTvShowUserInterface.cs`, `SlashCommands.txt`; the setting
goes through `DiscordSettings(Provider).cs`, `config/ChatClientsSettings.cs`,
`Controllers/ChatClients/*`, `SettingsTemplate.json` and the ClientApp `ChatClients.jsx`,
`ChatClientsActions.jsx`, `ChatClientsReducer.jsx`.

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

using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Requestrr.WebApi.RequestrrBot.ChatClients.Discord
{
    /// <summary>
    /// PH-Server fork, setting "PublicRequestConfirmations": /request searches are ephemeral
    /// (only the requester sees them and can dismiss them). Once a request is sent, the final card
    /// is posted publicly in the channel and the private one is deleted, so everyone can see what
    /// each person requested. With the setting off, upstream behavior applies.
    /// </summary>
    public static class DiscordPublicRequestMessage
    {
        /// <summary>
        /// Returns true if the public message was posted (and the ephemeral one removed).
        /// Returns false if the original response is not ephemeral or posting failed, so the
        /// caller should fall back to editing the original response as upstream does.
        /// </summary>
        public static async Task<bool> TryPublishAsync(DiscordInteraction interaction, string content, DiscordEmbed embed, DiscordComponent button)
        {
            try
            {
                if (!new DiscordSettingsProvider().Provide().PublicRequestConfirmations)
                {
                    return false;
                }

                var original = await interaction.GetOriginalResponseAsync();

                if (original?.Flags == null || !original.Flags.Value.HasFlag(MessageFlags.Ephemeral))
                {
                    return false;
                }

                var message = new DiscordMessageBuilder()
                    .WithContent($"{content}\n-# Requested by {interaction.User.Mention}")
                    .AddEmbed(embed)
                    .AddComponents(button)
                    .WithAllowedMentions(Mentions.None);

                await interaction.Channel.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PH-Server fork] Could not post public request message, falling back to ephemeral edit: {ex.Message}");
                return false;
            }

            try
            {
                await interaction.DeleteOriginalResponseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PH-Server fork] Could not delete ephemeral request message: {ex.Message}");
            }

            return true;
        }
    }
}

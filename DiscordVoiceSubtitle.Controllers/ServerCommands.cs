using DisCatSharp.Enums;

namespace DiscordVoiceSubtitle.Controllers
{
    public sealed class ServerCommands : ApplicationCommandsModule
    {
        [SlashCommand("ping", "Retrieve server ping")]
        public async Task Ping(InteractionContext context)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AsEphemeral()
                    .WithContent("Retrieving ping, please wait..."));

            await context.Channel.SendMessageAsync($"Pong: `{context.Client.Ping}");
        }

        [SlashCommand("about", "DiscordVoiceSubtitleBot info")]
        public async Task About(InteractionContext context)
        {
            await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

            var embed = new DiscordEmbedBuilder()
                .WithAuthor(context.Client.CurrentUser.UsernameWithDiscriminator, null, context.Client.CurrentUser.AvatarUrl)
                .WithTitle($"Information about {context.Client.CurrentUser.Username}")
                .AddField(new DiscordEmbedField("Number of Guilds:", $"'{context.Client.Guilds.Count}'", true))
                .AddField(new DiscordEmbedField("Number of Commands:", $"'{context.Client.GetApplicationCommands().RegisteredCommands[0].Value.Count}'", true))
                .AddField(new DiscordEmbedField("The Dev(s):", string.Join(", ",
                    context.Client.CurrentApplication.Team.Members.Select(x => $"{x.User.Mention}"))))
                .AddField(new DiscordEmbedField("Make with love", $"by {context.Client.CurrentApplication.TeamName}", true))
                .WithTimestamp(DateTime.Now)
                .WithColor(DiscordColor.Azure);

            await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
    }
}
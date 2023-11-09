namespace DiscordVoiceSubtitle.Bot
{
    internal class DiscordVoiceSubtitleBot : DiscordHostedService, IDiscordVoiceSubtitleBot
    {
        public DiscordVoiceSubtitleBot(IConfiguration config,
            ILogger<DiscordVoiceSubtitleBot> logger,
            IServiceProvider provider,
            IHostApplicationLifetime lifetime) : base(config, logger, provider, lifetime, "DiscordVoiceSubtitleBot") { }

        /// <summary>
        /// Attempts to register commands from our DiscordVoiceSubtitle.Bot Project
        /// </summary>
        void RegisterCommands(ApplicationCommandsExtension commandsExtension)
        {
            var registeredTypes = commandsExtension.RegisterApplicationCommandsFromAssembly();

            if (!registeredTypes.Any())
            {
                Logger.LogInformation($"Could not locate any commands to register...");
                return;
            }

            Logger.LogInformation("Registered the following command classes: \n\t {commands}", string.Join("\n\t", registeredTypes));
        }

        protected override Task ConfigureExtensionsAsync()
        {
            base.ConfigureExtensionsAsync();

            var commandsExtension = this.Client.UseApplicationCommands(new(this.ServiceProvider));
            RegisterCommands(commandsExtension);

            return Task.CompletedTask;
        }

        protected override void OnInitializationError(Exception ex)
        {
            Logger.LogError("Unexpected exception: {ex}", ex);
        }
    }
}
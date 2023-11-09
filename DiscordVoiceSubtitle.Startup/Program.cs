using DiscordVoiceSubtitle.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

#if DEBUG
builder.Configuration
    .AddUserSecrets(typeof(Program).Assembly);
#endif

// Services
builder.Services.AddDiscordVoiceSubtitleBotServices();

var app = builder.Build();

await app.StartAsync();
await app.WaitForShutdownAsync();
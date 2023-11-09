using DisCatSharp.Hosting.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiscordVoiceSubtitle.Bot
{
    public static class DiscordVoiceSubtitleBotServiceCollectionExtensions
    {
        /// <summary>
        /// Add DiscordVoiceSubtitleBot into the Dependency Injection Pipeline
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDiscordVoiceSubtitleBotServices(this IServiceCollection services)
        {
            services.AddDiscordHostedService<IDiscordVoiceSubtitleBot, DiscordVoiceSubtitleBot>();
            return services;
        }

        /// <summary>
        /// Register all commands from this project automatically
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="guildId">Optional GuildID to provide</param>
        /// <returns>List of command classes that were registered</returns>
        public static List<string> RegisterApplicationCommandsFromAssembly(this ApplicationCommandsExtension commands, ulong? guildId = null)
        {
            return RegisterApplicationCommandsFromAssembly(commands, Assembly.GetExecutingAssembly(), guildId);
        }

        /// <summary>
        /// Register all commands from assembly of type <typeparamref name="T"/> 
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="guildId">Optional GuildID to provide</param>
        /// <returns>List of command classes that were registered</returns>
        public static List<string> RegisterApplicationCommandsFromAssembly<T>(this ApplicationCommandsExtension commands, ulong? guildId = null)
        {
            return RegisterApplicationCommandsFromAssembly(commands, typeof(T).Assembly, guildId);
        }

        /// <summary>
        /// Register all commands from <paramref name="assembly"/> 
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="guildId">Optional GuildID to provide</param>
        /// <returns>List of command classes that were registered</returns>
        public static List<string> RegisterApplicationCommandsFromAssembly(this ApplicationCommandsExtension commands, Assembly assembly, ulong? guildId = null)
        {
            var results = assembly
                            .DefinedTypes
                            .Where(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo(typeof(ApplicationCommandsModule)))
                            .ToList();

            foreach (var type in results)
            {
                if (guildId.HasValue)
                {
                    commands.RegisterGuildCommands(type, guildId.Value);
                }
                else
                {
                    commands.RegisterGlobalCommands(type);
                }
            }

            return results.Select(x => x.Name).ToList();
        }

    }
}
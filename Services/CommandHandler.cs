using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTCGDiscordBot.Services
{
    public class CommandHandler
    {
        public static IServiceProvider _provider;
        public readonly DiscordSocketClient _discord;
        public readonly CommandService _commands;
        public readonly IConfigurationRoot _config;
        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider)
        {
            _provider = provider;
            _discord = discord;
            _commands = commands;
            _config = config;

            _discord.Ready += OnReady;
            _discord.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            try
            {
                var msg = arg as SocketUserMessage;

                if (msg.Author.IsBot)
                    return;
                var context = new SocketCommandContext(_discord, msg);

                int pos = 0;
                if (msg.HasStringPrefix(_config["prefix"], ref pos) || msg.HasMentionPrefix(_discord.CurrentUser, ref pos))
                {
                    Console.WriteLine($"User {msg.Author} has made a command request");
                    var result = await _commands.ExecuteAsync(context, pos, _provider);
                    if (!result.IsSuccess)
                    {
                        var reason = result.Error;

                        await context.Channel.SendMessageAsync($"The Following Error has Occured... \n {reason}");
                        //track error info in the console.... should probably add message and user info...
                        Console.WriteLine(reason);
                    }
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error Message: {ex.Message} \nError Stack Trace: {ex.StackTrace}");
            }
        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }
    }
}

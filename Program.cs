using System;
using System.Threading.Tasks;
namespace HTTCGDiscordBot
{
    class Program
    {
        public static async Task Main(string[] args)
            => await Startup.RunAsync(args);
    }
}

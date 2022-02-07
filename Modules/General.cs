using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HTTCGDiscordBot.Entities;
using Discord;

namespace HTTCGDiscordBot.Modules
{
    public class General : ModuleBase
    {
        [Command("gemmygreet")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Hello, Homies!"); 
        }

        [Command("getcard")]
        public async Task GetCard([Remainder] string cardName)
        {
            try
            {
                Embed embed;
                Card card = new Card();
                bool exactMatch = false;
                Console.WriteLine($"Searching for {cardName}...");
                string jsonResult;
                cardName = cardName.Trim().Replace(" ", "%20");
                string fullUrl = $"https://api.fabdb.net/cards?keywords={cardName}";


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullUrl);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var result = await reader.ReadToEndAsync();
                    jsonResult = result;
                }
                FaBDBSearchResponse searchResult = JsonConvert.DeserializeObject<FaBDBSearchResponse>(jsonResult);
                if (searchResult.Data.Length > 0 && searchResult.Data[0].ImageUrl != null)
                {
                    foreach (var result in searchResult.Data)
                    {
                        if (result.Name.ToLower() == cardName.Replace("%20", " ").ToLower())
                        {
                            exactMatch = true;
                            card = result;
                        }
                    }
                    if(exactMatch)
                    {
                        cardName = cardName.Replace("%20", " ");
                        Console.WriteLine($"results foud for {cardName}!  Returning results now...");
                        var builder = new EmbedBuilder()
                            .WithThumbnailUrl($"https://external-preview.redd.it/ssCwOsNtTs38yluHr1Ou55Qld7heN6FtMLkNu-UI_nY.jpg?auto=webp&s=9071f0a5e5c30f0b1976a5fb1f33bc1f4fdd107e")
                            .WithImageUrl(card.ImageUrl)
                            .WithFooter($"Powered by HometownTCG")
                            .WithTitle($"Direct Match Found for {cardName}")
                            .AddField("Card Name:", card.Name, true)
                            .AddField("Rarity:", card.Rarity, true)
                            .AddField("Text:", card.Text, false);
                        embed = builder.Build();
                    }
                    else
                    {
                        cardName = cardName.Replace("%20", " ");
                        Console.WriteLine($"results foud for {cardName}!  Returning results now...");
                        var builder = new EmbedBuilder()
                            .WithThumbnailUrl($"https://external-preview.redd.it/ssCwOsNtTs38yluHr1Ou55Qld7heN6FtMLkNu-UI_nY.jpg?auto=webp&s=9071f0a5e5c30f0b1976a5fb1f33bc1f4fdd107e")
                            .WithImageUrl(searchResult.Data[0].ImageUrl)
                            .WithFooter($"Powered by HometownTCG")
                            .WithTitle($"Indirect Match Found for {cardName}")
                            .AddField("Card Name:", searchResult.Data[0].Name, true)
                            .AddField("Rarity:", searchResult.Data[0].Rarity, true)
                            .AddField("Text:", searchResult.Data[0].Text, false);
                        embed = builder.Build();
                    }
                    await Context.Channel.SendMessageAsync(null, false, embed);
                }
                else
                {
                    Console.WriteLine($"results not foud for {cardName}!  informing user now...");
                    await Context.Channel.SendMessageAsync("Card not Found...");
                }
            }
            catch(Exception ex)
            {
                await Context.Channel.SendMessageAsync("Whoops... Something has gone HORRIBLY wrong!  Hopefully Josh Fixes me....");
                Console.WriteLine($"Error Message: {ex.Message} \nError Stack Trace: {ex.StackTrace}");
            }
        }
       
    }
}

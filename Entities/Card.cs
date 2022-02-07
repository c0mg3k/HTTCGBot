using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTCGDiscordBot.Entities
{
    public class Card
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("keywords")]
        public string[] KeyWords { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("rarity")]
        public string Rarity { get; set; }
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
        [JsonProperty("sideboardTotal")]
        public int SideboardTotal { get; set; }
    }
}

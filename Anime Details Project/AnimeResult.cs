using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Anime_Details_Project
{
    public class AnimeResult
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("episodes")]
        public int Episodes { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
    }
}

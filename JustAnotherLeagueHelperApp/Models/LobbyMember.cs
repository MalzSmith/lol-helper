using Newtonsoft.Json;

namespace JustAnotherLeagueHelperApp.Models
{
    public class LobbyMember
    {
        [JsonProperty(PropertyName = "summonerName")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "teamId")]
        public long Team { get; set; }
    }
}
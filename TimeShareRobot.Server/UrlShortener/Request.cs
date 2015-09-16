using Newtonsoft.Json;

namespace TimeShareRobot.Server.UrlShortenerApi
{
    public class Request
    {
        [JsonProperty(PropertyName = "longUrl")]
        public string LongUrl { get; set; }
    }
}

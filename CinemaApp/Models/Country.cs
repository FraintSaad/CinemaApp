using Newtonsoft.Json;

namespace CinemaApp.Models
{
    public class Country
    {
        [JsonProperty("country")]
        public string Name { get; set; }
    }
}

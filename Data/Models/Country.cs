using Newtonsoft.Json;

namespace Data.Models
{
    public class Country
    {
        [JsonProperty("country")]
        public string Name { get; set; }
    }
}

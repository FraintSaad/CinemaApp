using Newtonsoft.Json;

namespace Data.Models
{
    public class Genre
    {
        [JsonProperty("genre")]
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}

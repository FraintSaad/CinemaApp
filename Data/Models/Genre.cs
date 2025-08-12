using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

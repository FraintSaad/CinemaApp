using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

namespace CinemaApp.Models
{
    public class FilmModel
    {
        [JsonProperty("kinopoiskId")]
        public int KinopoiskId { get; set; }

        [JsonProperty("imdbId")]
        public string ImdbId { get; set; }

        [JsonProperty("nameRu")]
        public string NameRu { get; set; }

        [JsonProperty("nameEn")]
        public string NameEn { get; set; }

        [JsonProperty("nameOriginal")]
        public string NameOriginal { get; set; }

        [JsonProperty("countries")]
        public List<Country> Countries { get; set; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }

        [JsonProperty("ratingKinopoisk")]
        public double? RatingKinopoisk { get; set; }

        [JsonProperty("ratingImdb")]
        public double? RatingImdb { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("posterUrl")]
        public string PosterUrl { get; set; }

        [JsonProperty("posterUrlPreview")]
        public string PosterUrlPreview { get; set; }
    }


    public class Country
    {
        [JsonPropertyName("country")]
        public string Name { get; set; }
    }

    public class Genre
    {
        [JsonPropertyName("genre")]
        public string Name { get; set; }
    }


}
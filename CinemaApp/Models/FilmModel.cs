using System.Collections.Generic;

namespace CinemaApp.Models
{
    public class FilmModel
    {
        public int KinopoiskId { get; set; }
        public string? ImdbId { get; set; }
        public string? NameRu { get; set; }
        public string? NameEn { get; set; }
        public string? NameOriginal { get; set; }
        public List<Country>? Countries { get; set; }
        public List<Genre>? Genres { get; set; }
        public double? RatingKinopoisk { get; set; }
        public double? RatingImdb { get; set; }
        public int Year { get; set; }
        public string? Type { get; set; }
        public string? PosterUrl { get; set; }
        public string? PosterUrlPreview { get; set; }
    }
}
using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class FilmEntity
    {
        [Key]
        public int KinopoiskId { get; set; }
        public int Year { get; set; }
        public string NameRu { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string NameOriginal { get; set; } = string.Empty;
        public string Countries { get; set; } = string.Empty;
        public string Genres { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string PosterUrlPreview { get; set; } = string.Empty;
        public double? RatingKinopoisk { get; set; } = null;
        public double? RatingImdb { get; set; } = null;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public static FilmEntity FromFilmModel(FilmModel film)
        {
            var filmEntity = new FilmEntity
            {
                KinopoiskId = film.KinopoiskId,
                NameRu = film.NameRu ?? string.Empty,
                NameEn = film.NameEn ?? string.Empty,
                NameOriginal = film.NameOriginal ?? string.Empty,
                PosterUrlPreview = film.PosterUrlPreview ?? string.Empty,
                Countries = film.Countries != null ? string.Join(",", film.Countries.Select(c => c.Name)) : string.Empty,
                Genres = film.Genres != null ? string.Join(",", film.Genres) : string.Empty,
                RatingImdb = film.RatingImdb,
                RatingKinopoisk = film.RatingKinopoisk,
                Year = film.Year,
                Type = film.Type ?? string.Empty,
            };
            return filmEntity;
        }
    }
}

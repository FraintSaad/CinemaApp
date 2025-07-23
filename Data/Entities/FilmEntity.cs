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
    }
}

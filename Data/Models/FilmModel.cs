using Data.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Data.Models
{
    public class FilmModel : INotifyPropertyChanged
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
        [JsonIgnore]
        private bool _isInFavorites;
        [JsonIgnore]
        public bool IsInFavorites
        {
            get => _isInFavorites;
            set
            {
                if (_isInFavorites != value)
                {
                    _isInFavorites = value;
                    OnPropertyChanged(nameof(IsInFavorites));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static FilmModel FromFilmEntity(FilmEntity entity)
        {
            FilmModel filmModel = new FilmModel
            {

                KinopoiskId = entity.KinopoiskId,
                NameRu = entity.NameRu ?? string.Empty,
                NameEn = entity.NameEn ?? string.Empty,
                NameOriginal = entity.NameOriginal ?? string.Empty,
                PosterUrlPreview = entity.PosterUrlPreview ?? string.Empty,
                RatingImdb = entity.RatingImdb,
                RatingKinopoisk = entity.RatingKinopoisk,
                Year = entity.Year,
                Type = entity.Type ?? string.Empty,
                IsInFavorites = true
            };
            return filmModel;
        }
    }
}
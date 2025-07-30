using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace CinemaApp.Models
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


    }
}
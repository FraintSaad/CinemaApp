using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data.Models
{
    public class TmdbFilmModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? Overview { get; set; }
        public string? ReleaseDate { get; set; }
        public double? VoteAverage { get; set; }
        public int? VoteCount { get; set; }
        public double? Popularity { get; set; }
        public bool Adult { get; set; }              
        public bool Video { get; set; }               
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }    
        public List<int>? GenreIds { get; set; }

        private bool _isInFavorites;
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

        public int Year
        {
            get
            {
                if (DateTime.TryParse(ReleaseDate, out var dt))
                    return dt.Year;
                return 0;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

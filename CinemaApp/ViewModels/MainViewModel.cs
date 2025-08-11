using CinemaApp.Models;
using CinemaApp.Services;
using Data.Context;
using Data.Entities;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CinemaApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FilmsDbContext _dbContext;
        private readonly FilmService _filmService;

        private bool _isLoading = false;
        private bool _isNameDescending = true;
        private bool _isYearDescending = true;
        private bool _isRatingDescending = true;
        private int _page = 1;

        public ICommand AddToFavoritesCommand { get; }

        public ObservableCollection<FilmModel> Films { get; } = new ObservableCollection<FilmModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            _filmService = new FilmService();
            _dbContext = new FilmsDbContext();

            AddToFavoritesCommand = new RelayCommand<FilmModel>(AddToFavorites);
        }

        private void AddToFavorites(FilmModel film)
        {
            if (film == null || film.IsInFavorites)
                return;

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

            _dbContext.FavoriteFilms.Add(filmEntity);
            _dbContext.SaveChanges();

            film.IsInFavorites = true;
        }
    }
}
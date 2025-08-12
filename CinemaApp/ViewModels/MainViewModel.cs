using CinemaApp.Services;
using Data.Context;
using Data.Entities;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

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
        public ICommand DeleteFromFavoritesCommand { get; }
        public ICommand SortByNameCommand { get; }
        public ICommand SortByRatingCommand { get; }
        public ICommand SortByYearCommand { get; }
        public ObservableCollection<FilmModel> Films { get; } = new ObservableCollection<FilmModel>();
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            _filmService = new FilmService();
            _dbContext = new FilmsDbContext();
            AddToFavoritesCommand = new MyCommand(AddToFavoritesCommandHandler);
            DeleteFromFavoritesCommand = new MyCommand(DeleteFromFavoritesCommandHandler);
            SortByNameCommand = new MyCommand(SortByNameCommandHandler);
            SortByRatingCommand = new MyCommand(SortByRatingCommandHandler);
            SortByYearCommand = new MyCommand(SortByYearCommandHandler);
        }

        public async Task LoadFilmsAsync(int offset)
        {
            var films = await _filmService.GetFilmsAsync(offset);
            if (films == null)
            {
                return;
            }
            foreach (var film in films)
            {
                Films.Add(film);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Films)));
        }

        private void AddToFavoritesCommandHandler(object? parameter)
        {
            var film = parameter as FilmModel;
            AddToFavorites(film!);
        }

        private void AddToFavorites(FilmModel film)
        {
            if (film == null || film.IsInFavorites)
            {
                return;
            }
            _dbContext.FavoriteFilms.Add(FilmEntity.FromFilmModel(film));
            _dbContext.SaveChanges();
            film.IsInFavorites = true;
        }

        private void DeleteFromFavoritesCommandHandler(object? parameter)
        {
            var film = parameter as FilmModel;
            DeleteFromFavorites(film!);
        }

        private void DeleteFromFavorites(FilmModel film)
        {
            if (!film.IsInFavorites)
            {
                return;
            }
            var filmEntity = _dbContext.Find<FilmEntity>(film.KinopoiskId);
            _dbContext.ChangeTracker.Clear();
            _dbContext.FavoriteFilms.Remove(filmEntity);
            _dbContext.SaveChanges();
            film.IsInFavorites = false;
        }

        private void SortByNameCommandHandler(object? parameter)
        {
            SortByName();
        }
        private void SortByName()
        {
            if (Films == null || Films.Count == 0) return;

            List<FilmModel> sortedFilms;
            if (_isNameDescending)
            {
                sortedFilms = Films.OrderByDescending(f => string.IsNullOrEmpty(f.NameRu) ? f.NameOriginal : f.NameRu).ToList();
            }
            else
            {
                sortedFilms = Films.OrderBy(f => string.IsNullOrEmpty(f.NameRu) ? f.NameOriginal : f.NameRu).ToList();
            }
            _isNameDescending = !_isNameDescending;
            Films.Clear();
            foreach (var film in sortedFilms)
            {
                Films.Add(film);
            }
        }

        private void SortByRatingCommandHandler(object? parameter)
        {
            SortByRating();
        }

        private void SortByRating()
        {
            if (Films == null || Films.Count == 0)
            {
                return;
            }
            List<FilmModel> sortedFilms;
            if (_isRatingDescending)
            {
                sortedFilms = Films.OrderByDescending(f => f.RatingKinopoisk).ToList();
            }
            else
            {
                sortedFilms = Films.OrderBy(f => f.RatingKinopoisk).ToList();
            }
            _isRatingDescending = !_isRatingDescending;
            Films.Clear();
            foreach (var film in sortedFilms)
            {
                Films.Add(film);
            }
        }

        private void SortByYearCommandHandler(object? parameter)
        {
            SortByYear();
        }
        private void SortByYear()
        {
            if (Films == null || Films.Count == 0)
            {
                return;
            }
            List<FilmModel> sortedFilms;
            if (_isYearDescending)
            {
                sortedFilms = Films.OrderByDescending(f => f.Year).ToList();
            }
            else
            {
                sortedFilms = Films.OrderBy(f => f.Year).ToList();
            }
            _isYearDescending = !_isYearDescending;
            Films.Clear();
            foreach (var film in sortedFilms)
            {
                Films.Add(film);
            }
        }

        public async void LoadFilmsIfScrolledDownAsync(ScrollViewer scrollViewer)
        {
            if (scrollViewer == null || _isLoading)
            {
                return;
            }
            var scrollThreshhold = scrollViewer.ScrollableHeight * 0.8;
            try
            {
                if (scrollViewer.VerticalOffset >= scrollThreshhold)
                {
                    _isLoading = true;
                    await LoadFilmsAsync(_page++);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
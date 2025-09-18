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
        private readonly TmdbService _tmdbService;
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

        public ObservableCollection<TmdbFilmModel> Films { get; } = new ObservableCollection<TmdbFilmModel>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            _tmdbService = new TmdbService();
            _dbContext = new FilmsDbContext();

            AddToFavoritesCommand = new MyCommand(AddToFavoritesCommandHandler);
            DeleteFromFavoritesCommand = new MyCommand(DeleteFromFavoritesCommandHandler);
            SortByNameCommand = new MyCommand(SortByNameCommandHandler);
            SortByRatingCommand = new MyCommand(SortByRatingCommandHandler);
            SortByYearCommand = new MyCommand(SortByYearCommandHandler);

            // Загружаем первые фильмы сразу
           //_ = LoadFilmsAsync(_page);
        }

        public async Task LoadFilmsAsync(int page)
        {
            try
            {
                var films = await _tmdbService.GetPopularFilmsAsync();

                if (films == null) return;

                foreach (TmdbFilmModel film in films)
                {
                    // Проверяем, есть ли в избранном через KinopoiskId
                    var local = _dbContext.FavoriteFilms.FirstOrDefault(f => f.KinopoiskId == film.Id);
                    if (local != null) film.IsInFavorites = true;

                    Films.Add(film);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Films)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки фильмов: {ex.Message}");
                throw;
            }
        }

        private void AddToFavoritesCommandHandler(object? parameter)
        {
            var film = parameter as TmdbFilmModel;
            AddToFavorites(film!);
        }

        private void AddToFavorites(TmdbFilmModel film)
        {
            if (film == null || film.IsInFavorites) return;

            // Конвертируем TmdbFilmModel → FilmEntity для базы
            var entity = new FilmEntity
            {
                KinopoiskId = film.Id,
                NameRu = film.Title ?? string.Empty,
                NameOriginal = film.OriginalTitle ?? string.Empty,
                Year = film.Year,
                PosterUrlPreview = film.PosterPath ?? string.Empty,
                RatingKinopoisk = film.VoteAverage,
                RatingImdb = film.VoteAverage
            };

            _dbContext.FavoriteFilms.Add(entity);
            _dbContext.SaveChanges();
            film.IsInFavorites = true;
        }

        private void DeleteFromFavoritesCommandHandler(object? parameter)
        {
            var film = parameter as TmdbFilmModel;
            DeleteFromFavorites(film!);
        }

        private void DeleteFromFavorites(TmdbFilmModel film)
        {
            if (!film.IsInFavorites) return;

            var filmEntity = _dbContext.FavoriteFilms.FirstOrDefault(f => f.KinopoiskId == film.Id);
            if (filmEntity == null) return;

            _dbContext.ChangeTracker.Clear();
            _dbContext.FavoriteFilms.Remove(filmEntity);
            _dbContext.SaveChanges();
            film.IsInFavorites = false;
        }

        private void SortByNameCommandHandler(object? parameter) => SortByName();

        private void SortByName()
        {
            if (Films.Count == 0) return;

            var sorted = _isNameDescending
                ? Films.OrderByDescending(f => string.IsNullOrEmpty(f.Title) ? f.OriginalTitle : f.Title).ToList()
                : Films.OrderBy(f => string.IsNullOrEmpty(f.Title) ? f.OriginalTitle : f.Title).ToList();

            _isNameDescending = !_isNameDescending;
            Films.Clear();
            foreach (var f in sorted) Films.Add(f);
        }

        private void SortByRatingCommandHandler(object? parameter) => SortByRating();

        private void SortByRating()
        {
            if (Films.Count == 0) return;

            var sorted = _isRatingDescending
                ? Films.OrderByDescending(f => f.VoteAverage).ToList()
                : Films.OrderBy(f => f.VoteAverage).ToList();

            _isRatingDescending = !_isRatingDescending;
            Films.Clear();
            foreach (var f in sorted) Films.Add(f);
        }

        private void SortByYearCommandHandler(object? parameter) => SortByYear();

        private void SortByYear()
        {
            if (Films.Count == 0) return;

            var sorted = _isYearDescending
                ? Films.OrderByDescending(f => f.Year).ToList()
                : Films.OrderBy(f => f.Year).ToList();

            _isYearDescending = !_isYearDescending;
            Films.Clear();
            foreach (var f in sorted) Films.Add(f);
        }

        public async void LoadFilmsIfScrolledDownAsync(ScrollViewer scrollViewer)
        {
            if (scrollViewer == null || _isLoading) return;

            var scrollThreshold = scrollViewer.ScrollableHeight * 0.8;

            try
            {
                if (scrollViewer.VerticalOffset >= scrollThreshold)
                {
                    _isLoading = true;
                    await LoadFilmsAsync(++_page);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка подгрузки фильмов: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}

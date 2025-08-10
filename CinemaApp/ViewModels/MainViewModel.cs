using CinemaApp.Models;
using CinemaApp.Services;
using Data.Context;
using Data.Entities;
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

        public ObservableCollection<FilmModel> Films { get; } = new ObservableCollection<FilmModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            _filmService = new FilmService();
            _dbContext = new FilmsDbContext();


            LoadFilmsAsync(0);
        }

        private void addToFavorites(object sender, RoutedEventArgs e)
        {

            // Получить фильм на котором нажата кнопка
            var film = (((Button)sender).DataContext as FilmModel)!;
            // Создать FilmEntity и заполнить его данными из FilmModel

            var filmEntity = new FilmEntity
            {
                KinopoiskId = film.KinopoiskId,
                NameRu = film.NameRu ?? string.Empty,
                NameEn = film.NameEn ?? string.Empty,
                NameOriginal = film.NameOriginal ?? string.Empty,
                PosterUrlPreview = film.PosterUrlPreview ?? string.Empty,
                Countries = film.Countries != null ? string.Join(",", film.Countries.Select(c => c.Name).ToArray()) : string.Empty,
                Genres = film.Genres != null ? string.Join(",", film.Genres) : string.Empty,
                RatingImdb = film.RatingImdb,
                RatingKinopoisk = film.RatingKinopoisk,
                Year = film.Year,
                Type = film.Type ?? string.Empty,
            };

            CheckFavoritesStatus();

            if (film.IsInFavorites == false)
            {
                // Добавить FilmEntity в базу данных
                _dbContext.FavoriteFilms.Add(filmEntity);
                _dbContext.SaveChanges();
            }
            film.IsInFavorites = true;

        }
        private void deleteFromFavorites(object sender, RoutedEventArgs e)
        {
            var film = (((Button)sender).DataContext as FilmModel)!;
            var filmEntity = new FilmEntity
            {
                KinopoiskId = film.KinopoiskId,
                NameRu = film.NameRu ?? string.Empty,
                NameEn = film.NameEn ?? string.Empty,
                NameOriginal = film.NameOriginal ?? string.Empty,
                PosterUrlPreview = film.PosterUrlPreview ?? string.Empty,
                Countries = film.Countries != null ? string.Join(",", film.Countries.Select(c => c.Name).ToArray()) : string.Empty,
                Genres = film.Genres != null ? string.Join(",", film.Genres) : string.Empty,
                RatingImdb = film.RatingImdb,
                RatingKinopoisk = film.RatingKinopoisk,
                Year = film.Year,
                Type = film.Type ?? string.Empty,
            };
            CheckFavoritesStatus();
            if (film.IsInFavorites == true)
            {
                _dbContext.ChangeTracker.Clear();
                _dbContext.FavoriteFilms.Remove(filmEntity);
                _dbContext.SaveChanges();
            }
            film.IsInFavorites = false;
        }

        private void SetCursor(CoreCursorType cursorType)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.PointerCursor =
                new CoreCursor(cursorType, 0);
        }

        private void NavigateToFilm(FilmModel film)
        {
            // Здесь нужно использовать NavigationService, но пока просто оставим так
            // В реальном проекте лучше внедрить INavigationService
            (Window.Current.Content as Frame)?.Navigate(typeof(FilmPage), film);
        }

        private void NavigateToFavorites()
        {
            (Window.Current.Content as Frame)?.Navigate(typeof(FavoritesPage));
        }

        private void NavigateToMainPage()
        {
            (Window.Current.Content as Frame)?.Navigate(typeof(MainPage));
        }

        public async Task OnNavigatedToAsync()
        {
            await LoadFilmsAsync(0);
            CheckFavoritesStatus();
        }

        public async Task HandleScrollAsync(ScrollViewer scrollViewer)
        {
            if (scrollViewer == null || _isLoading)
                return;

            var scrollThreshold = scrollViewer.ScrollableHeight * 0.8;

            if (scrollViewer.VerticalOffset >= scrollThreshold)
            {
                _isLoading = true;
                await LoadFilmsAsync(_page++);
                _isLoading = false;
                CheckFavoritesStatus();
            }
        }

        public void SortByName()
        {
            if (Films.Count == 0) return;

            var sortedFilms = _isNameDescending
                ? Films.OrderByDescending(f => string.IsNullOrEmpty(f.NameRu) ? f.NameOriginal : f.NameRu).ToList()
                : Films.OrderBy(f => string.IsNullOrEmpty(f.NameRu) ? f.NameOriginal : f.NameRu).ToList();

            _isNameDescending = !_isNameDescending;
            UpdateFilmsCollection(sortedFilms);
        }

        public void SortByRating()
        {
            if (Films.Count == 0) return;

            var sortedFilms = _isRatingDescending
                ? Films.OrderByDescending(f => f.RatingKinopoisk).ToList()
                : Films.OrderBy(f => f.RatingKinopoisk).ToList();

            _isRatingDescending = !_isRatingDescending;
            UpdateFilmsCollection(sortedFilms);
        }

        public void SortByYear()
        {
            if (Films.Count == 0) return;

            var sortedFilms = _isYearDescending
                ? Films.OrderByDescending(f => f.Year).ToList()
                : Films.OrderBy(f => f.Year).ToList();

            _isYearDescending = !_isYearDescending;
            UpdateFilmsCollection(sortedFilms);
        }

        private async Task LoadFilmsAsync(int offset)
        {
            var films = await _filmService.GetFilmsAsync(offset);
            if (films == null) return;

            foreach (var film in films)
            {
                Films.Add(film);
            }
        }

        private void CheckFavoritesStatus()
        {
            foreach (var film in Films)
            {
                var local = _dbContext.Set<FilmEntity>().FirstOrDefault(item => item.KinopoiskId.Equals(film.KinopoiskId));
                film.IsInFavorites = local != null;
            }
        }

        private void UpdateFilmsCollection(List<FilmModel> sortedFilms)
        {
            Films.Clear();
            foreach (var film in sortedFilms)
            {
                Films.Add(film);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
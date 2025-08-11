using CinemaApp.Models;
using CinemaApp.Services;
using Data.Context;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace CinemaApp
{
    public sealed partial class MainPage : Page
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly FilmsDbContext _dbContext;
        private readonly FilmService _filmService;

        public ObservableCollection<FilmModel> Films { get; set; } = new ObservableCollection<FilmModel>();

        private bool _isLoading = false;
        private bool _isNameDescending = true;
        private bool _isYearDescending = true;
        private bool _isRatingDescending = true;
        int page = 1;


        public MainPage()
        {
            this.InitializeComponent();
            DataContext = this;
            _filmService = new FilmService();
            _dbContext = new FilmsDbContext();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await LoadFilmsAsync(0);


        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
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
                    await LoadFilmsAsync(page++);
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

        private void FavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavoritesPage));
        }

        private void MainPageBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void addToFavoritesBtn_Click(object sender, RoutedEventArgs e)
        {

            // Получить фильм на котором нажата кнопка
            var film = (((Button)sender).DataContext as FilmModel)!;
            addToFavorites(film);

        }

        private void addToFavorites(FilmModel film)
        {
            if (film.IsInFavorites)
            {
                return;
            }

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



            // Добавить FilmEntity в базу данных
            _dbContext.FavoriteFilms.Add(filmEntity);
            _dbContext.SaveChanges();

            film.IsInFavorites = true;

        }
        private void DeleteFromFavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            var film = (((Button)sender).DataContext as FilmModel)!;
            DeleteFromFavorites(film);
           
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
        }

        private void SortByNameBtn_Click(object sender, RoutedEventArgs e)
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
                if (!Films.Contains(film))
                {
                    Films.Add(film);
                }
            }
        }

        private void SortByRatingBtn_Click(object sender, RoutedEventArgs e)
        {
            SortByRating();
        }

        private void SortByRating()
        {
            if (Films == null || Films.Count == 0) return;

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
                if (!Films.Contains(film))
                {
                    Films.Add(film);
                }
            }
        }
        private void SortByYearBtn_Click(object sender, RoutedEventArgs e)
        {
            SortByYear();
        }

        private void SortByYear()
        {
            if (Films == null || Films.Count == 0) return;

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
                if (!Films.Contains(film))
                {
                    Films.Add(film);
                }
            }
        }

        private void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FilmModel film)
            {
                Frame.Navigate(typeof(FilmPage), film);
            }
        }

        private void CursorEntered_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }

        private void CursorExited_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }
    }
}

using CinemaApp.Models;
using CinemaApp.Services;
using CinemaApp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Data.Context;
using Data.Entities;
using System.Linq;
using System.Collections.Generic;

namespace CinemaApp
{
    public sealed partial class MainPage : Page
    {
        private readonly FilmService _filmService;
        private readonly FilmsViewModel _filmsViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = _filmsViewModel;
            _filmService = new FilmService();
            _filmsViewModel = new FilmsViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await _filmsViewModel.LoadFilmsAsync(0);
        }

        //private async Task ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        //{
        //    var scrollViewer = sender as ScrollViewer;
        //    if (scrollViewer == null || _isLoading)
        //    {
        //        return;
        //    }

        //    var scrollThreshhold = scrollViewer.ScrollableHeight * 0.8;

        //    try
        //    {
        //        if (scrollViewer.VerticalOffset >= scrollThreshhold)
        //        {
        //            _isLoading = true;
        //            _filmsViewModel.pages++;
        //            _filmsViewModel.LoadFilmsAsync();
        //        }
        //    }
        //    catch (Exception ex) 
        //    {
        //        Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
        //        _filmsViewModel.pages--;
        //    }
        //    finally
        //    {
        //        _isLoading = false;
        //    }
        //}

        private void FavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavoritesPage));
        }

        private void MainPageBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addToFavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            // Добавить фильм в избранное

            // Получить фильм на котором нажата кнопка
            var film = (((Button)sender).DataContext as FilmModel)!;

            // Создать FilmEntity и заполнить его данными из FilmModel
            var filmEntity = new FilmEntity
            {
                KinopoiskId = film.KinopoiskId,
                NameRu = film.NameRu ?? string.Empty,
                NameEn = film.NameEn ?? string.Empty,
                NameOriginal = film.NameOriginal  ?? string.Empty,
                PosterUrlPreview = film.PosterUrlPreview ?? string.Empty,
                Countries = film.Countries != null ? string.Join(",", film.Countries.Select(c => c.Name).ToArray()) : string.Empty,
                Genres = film.Genres != null ? string.Join(",", film.Genres) : string.Empty,
                RatingImdb = film.RatingImdb,
                RatingKinopoisk = film.RatingKinopoisk,
                Year = film.Year,
                Type = film.Type ?? string.Empty,
            };

            // Добавить FilmEntity в базу данных
            using (var dbContext = new FilmsDbContext())
            {
                dbContext.FavoriteFilms.Add(filmEntity);
                dbContext.SaveChanges();
            }
        }
    }
}

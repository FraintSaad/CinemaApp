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
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CinemaApp
{
    public sealed partial class MainPage : Page
    {
        private readonly FilmService _filmService;
        private readonly FilmsViewModel _filmsViewModel;
        private readonly FilmsDbContext _dbContext;
        private bool _isLoading = false;

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = _filmsViewModel;
            _filmService = new FilmService();
            _filmsViewModel = new FilmsViewModel();
            _dbContext = new FilmsDbContext();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await _filmsViewModel.LoadFilmsAsync(0);

        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            int page = 1;
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
                    page++;
                    _isLoading = true;
                    await _filmsViewModel.LoadFilmsAsync(page);
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
            if (_dbContext.FavoriteFilms.Contains(filmEntity))
            {
                ((Button)sender).Content = "Уже добавлен";
                return;
            }
            _dbContext.FavoriteFilms.Add(filmEntity);
            _dbContext.SaveChanges();
        }
    }
}

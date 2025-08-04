using CinemaApp.Models;
using Data.Context;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Net.WebRequestMethods;

namespace CinemaApp
{
    public sealed partial class FilmPage : Page
    {
        public FilmModel CurrentFilm { get; set; }
        private readonly FilmsDbContext _dbContext = new FilmsDbContext();

        public FilmPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is FilmModel film)
            {
                CurrentFilm = film;
                CheckIfFavorite();
            }
            else if (e.Parameter is FilmEntity entity)
            {
                {
                    FilmModel convertedFilm = new FilmModel
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
                    CurrentFilm = convertedFilm;
                }
            }
        }

        public string GetCountriesString(List<Country> countries)
        {
            return countries != null ? string.Join(", ", countries.Select(c => c.Name)) : string.Empty;
        }

        public string GetGenresString(List<Genre> genres)
        {
            return genres != null ? string.Join(", ", genres.Select(c => c.Name)) : string.Empty;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void addToFavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentFilm == null) return;

            var filmEntity = new FilmEntity
            {
                KinopoiskId = CurrentFilm.KinopoiskId,
                NameRu = CurrentFilm.NameRu ?? string.Empty,
                NameEn = CurrentFilm.NameEn ?? string.Empty,
                NameOriginal = CurrentFilm.NameOriginal ?? string.Empty,
                PosterUrlPreview = CurrentFilm.PosterUrlPreview ?? string.Empty,
                RatingImdb = CurrentFilm.RatingImdb,
                RatingKinopoisk = CurrentFilm.RatingKinopoisk,
                Year = CurrentFilm.Year,
                Type = CurrentFilm.Type ?? string.Empty
            };

            if (CurrentFilm.IsInFavorites == false)
            {
                // Добавить FilmEntity в базу данных
                _dbContext.FavoriteFilms.Add(filmEntity);
                _dbContext.SaveChanges();
            }
            CurrentFilm.IsInFavorites = true;
        }

        private void DeleteFromFavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentFilm == null) return;

            var filmEntity = _dbContext.FavoriteFilms
                .FirstOrDefault(f => f.KinopoiskId == CurrentFilm.KinopoiskId);

            if (filmEntity != null)
            {
                _dbContext.FavoriteFilms.Remove(filmEntity);
                _dbContext.SaveChanges();

                CurrentFilm.IsInFavorites = false;
            }
        }

        private void CheckIfFavorite()
        {
            if (CurrentFilm == null) return;

            CurrentFilm.IsInFavorites = _dbContext.FavoriteFilms
                .Any(f => f.KinopoiskId == CurrentFilm.KinopoiskId);
        }
    }
}

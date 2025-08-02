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

namespace CinemaApp
{
    public sealed partial class FilmPage : Page
    {
        public FilmModel CurrentFilm { get; set; }


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
            }
            else if (e.Parameter is FilmEntity entity) {
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
                } }
        }

        public string GetCountriesString(List<Country> countries)
        {
            return countries != null
                ? string.Join(", ", countries.Select(c => c.Name))
                : string.Empty;
        }

        public string GetGenresString(List<Genre> genres)
        {
            return genres != null
                ? string.Join(", ", genres.Select(c => c.Name))
                : string.Empty;
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

        }
        private void DeleteFromFavoritesBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

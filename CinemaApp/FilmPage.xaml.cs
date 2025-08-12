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

        private string GetCountriesString(List<Country> countries)
        {
            return countries != null ? string.Join(", ", countries.Select(c => c.Name)) : string.Empty;
        }

        private string GetGenresString(List<Genre> genres)
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

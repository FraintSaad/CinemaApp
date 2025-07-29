using CinemaApp.Models;
using CinemaApp.ViewModels;
using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class FavoritesPage : Page
    {
        private readonly FavoritesViewModel _favoritesViewModel;
        private readonly FilmsDbContext _dbContext;
        public FilmsDbContext dbContext { get; set; }
        public FavoritesPage()
        {
            this.InitializeComponent();
            _favoritesViewModel = new FavoritesViewModel();
            _dbContext = new FilmsDbContext();
            Loaded += async (s, e) => await _favoritesViewModel.LoadFavoritesAsync();
        }

        private void FavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavoritesPage));

        }

        private void MainPageBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));

        }

        private void RemoveFromFavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
           
            var film = (((Button)sender).DataContext as FilmEntity)!;
            _dbContext.FavoriteFilms.Remove(film);
            _dbContext.SaveChanges();
            _favoritesViewModel?.LoadFavoritesAsync();

        }
    }
}

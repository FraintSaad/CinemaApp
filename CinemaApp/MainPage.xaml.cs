using CinemaApp.Models;
using CinemaApp.Shared.Services;
using CinemaApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            await _filmsViewModel.LoadFilmsAsync();

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

        private void FavoritesBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavoritesPage));
        }

        private void MainPageBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void addToFavoritesBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}

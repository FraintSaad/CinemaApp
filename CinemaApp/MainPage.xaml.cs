using CinemaApp.Models;
using CinemaApp.ViewModels;
using Data.Entities;
using System;
using System.Collections.Generic;
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
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        //private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        //{
        //    var scrollViewer = sender as ScrollViewer;
        //    LoadFilmsIfScrolledDownAsync(scrollViewer);
        //}

        //private async void LoadFilmsIfScrolledDownAsync(ScrollViewer scrollViewer)
        //{
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
        //            await LoadFilmsAsync(page++);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
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
            Frame.Navigate(typeof(MainPage));
        }

        
        
        
        
        

        
        
        
        
        

        
        
        
        
        
        
        
        
        
        

        
        


        
        
        
        

        
        
        

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

        
        
        
        

        
        
        

        
        
        

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

        
        
        

        
        
        

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

        //private void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    if (e.ClickedItem is FilmModel film)
        //    {
        //        Frame.Navigate(typeof(FilmPage), film);
        //    }
        //}

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
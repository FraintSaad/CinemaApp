using CinemaApp.Models;
using CinemaApp.ViewModels;
using Data.Entities;
using Data.Models;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            MainViewModel vm = (MainViewModel)DataContext;
            vm.LoadFilmsIfScrolledDownAsync(scrollViewer);
        }

        private void FavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavoritesPage));
        }

        private void MainPageBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
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
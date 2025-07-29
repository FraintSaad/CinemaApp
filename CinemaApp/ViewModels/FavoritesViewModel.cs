using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.ViewModels
{
    internal class FavoritesViewModel
    {
        public ObservableCollection<FilmEntity> FavoriteFilms { get; set; } = new ObservableCollection<FilmEntity>();

        public async Task LoadFavoritesAsync()
        {
            var db = new FilmsDbContext();
            var films = await db.FavoriteFilms.ToListAsync();

            FavoriteFilms.Clear();
            foreach (var film in films)
            {
                FavoriteFilms.Add(film);
            }
        }
    }
}

using CinemaApp.Models;
using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace CinemaApp.ViewModels
{
    public class FilmPageViewModel
    {
        public FilmModel CurrentFilm { get; set; }
        private readonly FilmsDbContext _dbContext = new FilmsDbContext();

        public ICommand AddToFavoritesCommand { get; }
        public ICommand DeleteFromFavoritesCommand { get; }

        public FilmPageViewModel()
        {
            AddToFavoritesCommand = new MyCommand(AddToFavoritesCommandHandler);
            DeleteFromFavoritesCommand = new MyCommand(DeleteFromFavoritesCommandHandler);
        }

        private void AddToFavoritesCommandHandler(object? parameter)
        {
            AddToFavorites();
        }
        private void AddToFavorites()
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
            // Добавить FilmEntity в базу данных
            _dbContext.FavoriteFilms.Add(filmEntity);
            _dbContext.SaveChanges();

            CurrentFilm.IsInFavorites = true;
        }

        private void DeleteFromFavoritesCommandHandler(object? parameter)
        {
            DeleteFromFavorites();
        }
        private void DeleteFromFavorites()
        {
            if (CurrentFilm == null)
            {
                return;
            }

            var filmEntity = _dbContext.FavoriteFilms
                .FirstOrDefault(f => f.KinopoiskId == CurrentFilm.KinopoiskId);


            _dbContext.FavoriteFilms.Remove(filmEntity);
            _dbContext.SaveChanges();

            CurrentFilm.IsInFavorites = false;

        }
    }
}

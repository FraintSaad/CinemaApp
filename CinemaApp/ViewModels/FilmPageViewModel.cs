using CinemaApp.Models;
using Data.Context;
using Data.Entities;
using Data.Models;
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
        public string CountriesString => GetCountriesString(CurrentFilm?.Countries);
        public string GenresString => GetGenresString(CurrentFilm?.Genres);

        public FilmPageViewModel()
        {
            AddToFavoritesCommand = new MyCommand(AddToFavoritesCommandHandler);
            DeleteFromFavoritesCommand = new MyCommand(DeleteFromFavoritesCommandHandler);
        }

        public void ConvertCurrentFilmType(object navigationParameter)
        {
            if (navigationParameter is FilmModel film)
            {
                CurrentFilm = film;
            }
            else if (navigationParameter is FilmEntity entity)
            {
                CurrentFilm = FilmModel.FromFilmEntity(entity);
            }
        }

        private void AddToFavoritesCommandHandler(object? parameter)
        {
            AddToFavorites();
        }
        private void AddToFavorites()
        {
            if (CurrentFilm == null)
            {
                return;
            }
            _dbContext.FavoriteFilms.Add(FilmEntity.FromFilmModel(CurrentFilm));
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

        private string GetCountriesString(List<Country> countries)
        {
            return countries != null ? string.Join(", ", countries.Select(c => c.Name)) : string.Empty;
        }

        private string GetGenresString(List<Genre> genres)
        {
            return genres != null ? string.Join(", ", genres.Select(c => c.Name)) : string.Empty;
        }
    }
}

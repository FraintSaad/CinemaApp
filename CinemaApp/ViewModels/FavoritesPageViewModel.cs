using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinemaApp.ViewModels
{
    public class FavoritesPageViewModel : INotifyPropertyChanged
    {
        private readonly FilmsDbContext _dbContext;
        public ICommand RemoveFromFavoritesCommand { get; }
        public ObservableCollection<FilmEntity> FavoriteFilms { get; set; } = new ObservableCollection<FilmEntity>();
        public event PropertyChangedEventHandler? PropertyChanged;
        public FilmsDbContext dbContext { get; set; }
       
        public FavoritesPageViewModel()
        {
            _dbContext = new FilmsDbContext();
            RemoveFromFavoritesCommand = new MyCommand(RemoveFromFavoritesCommandHandler);
            LoadFavoritesAsync().ConfigureAwait(false);
        }

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

        private void RemoveFromFavoritesCommandHandler(object? parameter)
        {
            var film = parameter as FilmEntity;
            RemoveFromFavorites(film!);
        }
        private void RemoveFromFavorites(FilmEntity film)
        {
            _dbContext.FavoriteFilms.Remove(film);
            _dbContext.SaveChanges();
            FavoriteFilms.Remove(film);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FavoriteFilms)));
        }
    }
}

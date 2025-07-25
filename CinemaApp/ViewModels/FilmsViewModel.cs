using CinemaApp.Models;
using CinemaApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CinemaApp.ViewModels
{
    public partial class FilmsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly FilmService _filmService;
        public ObservableCollection<FilmModel> Films { get; set; } = new ObservableCollection<FilmModel>();
        public FilmsViewModel()
        {
            _filmService = new FilmService();
        }


        public async Task LoadFilmsAsync(int offset)
        {
            var films = await _filmService.GetFilmsAsync(offset);
            if (films == null)
            {
                return;
            }

            foreach (var film in films)
            {
                Films.Add(film);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

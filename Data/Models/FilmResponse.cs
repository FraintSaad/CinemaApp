using Data.Models;
using System.Collections.Generic;

namespace CinemaApp.Models
{
    public class FilmResponse
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<TmdbFilmModel>? Items { get; set; }
    }
}
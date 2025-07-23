using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CinemaApp.Models
{
    public class FilmResponse
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<FilmModel>? Items { get; set; }
    }
}
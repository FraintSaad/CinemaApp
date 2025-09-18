using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPresentation.Models
{
    public class PopularMoviesResponse
    {
        public int Page { get; set; }
        public List<TmdbFilmItem> Results { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}

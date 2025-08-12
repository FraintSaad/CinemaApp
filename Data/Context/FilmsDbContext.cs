using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class FilmsDbContext : DbContext
    {
        public FilmsDbContext(DbContextOptions options) : base(options) { }
        public FilmsDbContext() : base() { }
        public DbSet<FilmEntity> FavoriteFilms { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var connectionString = $"Data Source={Path.Combine(localAppDataPath, "CinemaAppDB.db")}";
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}

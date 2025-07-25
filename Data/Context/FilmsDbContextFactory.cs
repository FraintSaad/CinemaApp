using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context
{
    internal class FilmsDbContextFactory : IDesignTimeDbContextFactory<FilmsDbContext>
    {
        public FilmsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FilmsDbContext>();
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var connectionString = $"Data Source={Path.Combine(localAppDataPath, "CinemaAppDB.db")}";

            optionsBuilder.UseSqlite(connectionString);

            return new FilmsDbContext(optionsBuilder.Options);
        }
    }
}

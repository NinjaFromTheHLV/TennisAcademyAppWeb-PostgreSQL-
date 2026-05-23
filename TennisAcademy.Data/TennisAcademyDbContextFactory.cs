using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TennisAcademyApp.Data
{
    public class TennisAcademyDbContextFactory : IDesignTimeDbContextFactory<TennisAcademyDbContext>
    {
        public TennisAcademyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TennisAcademyDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                                   ?? "Host=localhost;Database=TennisAcademyDb;Username=postgres;Password=mypassword";

            optionsBuilder.UseNpgsql(connectionString);

            return new TennisAcademyDbContext(optionsBuilder.Options);
        }
    }
}
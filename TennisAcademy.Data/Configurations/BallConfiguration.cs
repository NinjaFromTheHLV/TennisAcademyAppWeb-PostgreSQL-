using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Configurations
{
    public class BallConfiguration : IEntityTypeConfiguration<Ball>
    {
        public void Configure(EntityTypeBuilder<Ball> config)
        {
            config
                .Property(b => b.Price)
                .HasPrecision(18, 2);
        }
    }
}

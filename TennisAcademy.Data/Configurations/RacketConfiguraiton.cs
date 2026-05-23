using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Configurations
{
    public class RacketConfiguraiton : IEntityTypeConfiguration<Racket>
    {
        public void Configure(EntityTypeBuilder<Racket> config)
        {
            config
                .Property(r => r.Price)
                .HasPrecision(18, 2);
        }
    }
}
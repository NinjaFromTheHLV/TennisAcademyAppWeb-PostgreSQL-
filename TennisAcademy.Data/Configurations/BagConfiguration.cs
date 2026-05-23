using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Configurations
{
    public class BagConfiguration : IEntityTypeConfiguration<Bag>
    {
        public void Configure(EntityTypeBuilder<Bag> config)
        {
            config
                .Property(b => b.Price)
                .HasPrecision(18, 2);
        }
    }
}

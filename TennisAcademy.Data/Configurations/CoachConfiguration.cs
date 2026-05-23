using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants.Coach;

namespace TennisAcademyApp.Data.Configurations
{
    public class CoachConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> config)
        {
            config
                .Property(c => c.Name)
                .IsUnicode(true);

            config
                .Property(c => c.Description)
                .HasMaxLength(CoachDescriptionMaxLenght);
        }
    }
}

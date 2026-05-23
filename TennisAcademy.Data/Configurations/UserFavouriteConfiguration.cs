
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Configurations
{
    public class UserFavouriteConfiguration : IEntityTypeConfiguration<UserFavourite>
    {
        public void Configure(EntityTypeBuilder<UserFavourite> config)
        {
            config
                .HasKey(uc => new { uc.UserId, uc.CoachId });

            config
                .HasOne(uc => uc.User)
                .WithMany()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            config
                .HasOne(uc => uc.Coach)
                .WithMany(uc => uc.UserFavourites)
                .HasForeignKey(uc => uc.CoachId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

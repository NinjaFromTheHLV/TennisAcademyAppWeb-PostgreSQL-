using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Configurations
{
    public class BallCartConfiguration : IEntityTypeConfiguration<BallCart>
    {
        public void Configure(EntityTypeBuilder<BallCart> config)
        {
            config.HasKey(bc => new { bc.BallId, bc.UserId, bc.IsGift });

            config
                .HasOne(bc => bc.Ball)
                .WithMany()
                .HasForeignKey(bc => bc.BallId)
                .OnDelete(DeleteBehavior.Restrict);

            config
                .HasOne(bc => bc.User)
                .WithMany()
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

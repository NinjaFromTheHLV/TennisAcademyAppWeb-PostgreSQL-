using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Configurations
{
    public class RacketCartConfiguration : IEntityTypeConfiguration<RacketCart>
    {
        public void Configure(EntityTypeBuilder<RacketCart> config)
        {
            config.HasKey(rc => new { rc.RacketId, rc.UserId, rc.IsGift });

            config
                .HasOne(rc => rc.Racket)
                .WithMany(r => r.RacketCart)
                .HasForeignKey(rc => rc.RacketId)
                .OnDelete(DeleteBehavior.Restrict);

            config
                .HasOne(rc => rc.User)
                .WithMany()
                .HasForeignKey(rc => rc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Configurations
{
    public class BagCartConfiguration : IEntityTypeConfiguration<BagCart>
    {
        public void Configure(EntityTypeBuilder<BagCart> config)
        {
            config.HasKey(bc => new { bc.BagId, bc.UserId, bc.IsGift });

            config
                .HasOne(bc => bc.Bag)
                .WithMany(b => b.BagCarts)
                .HasForeignKey(bc => bc.BagId)
                .OnDelete(DeleteBehavior.Restrict);


            config
                .HasOne(bc => bc.User)
                .WithMany()
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

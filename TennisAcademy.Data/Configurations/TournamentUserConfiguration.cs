using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Configurations
{
    public class TournamentUserConfiguration : IEntityTypeConfiguration<TournamentUser>
    {
        public void Configure(EntityTypeBuilder<TournamentUser> builder)
        {
            builder.HasKey(tu => new { tu.TournamentId, tu.UserId });

            builder.HasOne(tu => tu.Tournament)
                .WithMany(t => t.Participants)
                .HasForeignKey(tu => tu.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tu => tu.User)
                .WithMany()
                .HasForeignKey(tu => tu.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
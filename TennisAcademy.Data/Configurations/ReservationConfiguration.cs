using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants.Reservation;

namespace TennisAcademyApp.Data.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> config)
        {

            config
                .Property(r => r.Note)
                .HasMaxLength(PlayerNotesMaxLenght);

            config
                .HasOne(r => r.Coach)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.CoachId)
                .OnDelete(DeleteBehavior.NoAction);

            config
                .HasOne(r => r.Surface)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.SurfaceId);

            config
                .HasOne(r => r.Player)
                .WithMany()
                .HasForeignKey(r => r.PlayerId);

            config
                .HasOne(r => r.TrainingType)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.TrainingTypeId);

            config
                .Property(r => r.IsDeleted)
                .HasDefaultValue(false);

            config
                .HasQueryFilter(c => c.IsDeleted == false);

        }
    }
}

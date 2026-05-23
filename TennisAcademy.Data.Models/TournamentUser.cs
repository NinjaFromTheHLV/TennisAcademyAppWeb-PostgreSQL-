using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TennisAcademyApp.Data.Models
{
    public class TournamentUser
    {
        public int TournamentId { get; set; }

        [ForeignKey(nameof(TournamentId))]
        public Tournament Tournament { get; set; } = null!;

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public DateTime EnrolledOn { get; set; } = DateTime.UtcNow;
    }
}
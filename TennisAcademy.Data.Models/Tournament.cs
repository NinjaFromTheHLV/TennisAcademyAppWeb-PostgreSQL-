using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisAcademyApp.Data.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TitleBg { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string DescriptionBg { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EntryFee { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public TournamentCategory Category { get; set; } = null!;

        public ICollection<TournamentUser> Participants { get; set; } = new List<TournamentUser>();
    }
}
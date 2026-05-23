using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    public class TournamentCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string NameBg { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
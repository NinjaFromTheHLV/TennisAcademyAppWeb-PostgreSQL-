using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    [Comment("Tennis Academy Trainings")]
    public class TrainingType
    {
        [Comment("Training Type Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Training Type Name in English")]
        [Required]
        public string Name { get; set; } = null!;

        [Comment("Training Type Name in Bulgarian")]
        [Required]
        public string NameBg { get; set; } = null!;

        public virtual ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
    }
}
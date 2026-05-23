using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisAcademyApp.Data.Models
{
    public class Coach
    {
        [Key]
        public int CoachId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string NameBg { get; set; } = null!;

        [Required]
        public int Age { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string DescriptionBg { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Nationality { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string NationalityBg { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;
        [Comment("Identity User Identifier linked to this coach")]
        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Reservation> Reservations { get; set; }

        public virtual ICollection<UserFavourite> UserFavourites { get; set; }
    }
}
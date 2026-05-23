using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TennisAcademyApp.Data.Models
{
    [Comment("Player Reservations")]
    public class Reservation
    {
        [Comment("Reservation Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Player Notes in English")]
        public string? Note { get; set; }

        [Comment("Player Notes in Bulgarian")]
        public string? NoteBg { get; set; }

        [Comment("Choosing a surface")]
        [Required]
        public int SurfaceId { get; set; }

        public virtual Surface Surface { get; set; } = null!;

        [Comment("Choosing a coach")]
        [Required]
        public int CoachId { get; set; }

        public virtual Coach Coach { get; set; } = null!;

        [Comment("Choosing a training type")]
        [Required]
        public int TrainingTypeId { get; set; }

        public virtual TrainingType TrainingType { get; set; } = null!;

        [Comment("Player Identifer")]
        [Required]
        public string PlayerId { get; set; } = null!;

        public virtual ApplicationUser Player { get; set; } = null!;

        [Comment("Duration of the session")]
        [Required]
        public int Duration { get; set; }

        [Comment("Date Select")]
        [Required]
        public DateTime Date { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
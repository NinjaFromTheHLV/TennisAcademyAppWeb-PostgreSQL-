using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    [Comment("Rackets Shop")]
    public class Racket
    {
        [Key]
        [Comment("Racket Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Racket Brand in English")]
        public string Brand { get; set; } = null!;

        [Required]
        [Comment("Racket Brand in Bulgarian")]
        public string BrandBg { get; set; } = null!;

        [Required]
        [Comment("Racket Model in English")]
        public string Model { get; set; } = null!;

        [Required]
        [Comment("Racket Model in Bulgarian")]
        public string ModelBg { get; set; } = null!;

        [Required]
        [Comment("Racket Price")]
        public decimal Price { get; set; }

        [Required]
        [Comment("Available in stock")]
        public int Quantity { get; set; }

        [Required]
        [Comment("Racket Image")]
        public string ImageUrl { get; set; } = null!;

        public virtual ICollection<RacketCart> RacketCart { get; set; } = new HashSet<RacketCart>();
    }
}
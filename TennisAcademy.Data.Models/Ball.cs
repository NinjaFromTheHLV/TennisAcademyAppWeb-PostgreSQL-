using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    [Comment("Balls Shop")]
    public class Ball
    {
        [Key]
        [Comment("Ball Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Ball Brand in English")]
        public string Brand { get; set; } = null!;

        [Required]
        [Comment("Ball Brand in Bulgarian")]
        public string BrandBg { get; set; } = null!;

        [Required]
        [Comment("Ball Model in English")]
        public string Model { get; set; } = null!;

        [Required]
        [Comment("Ball Model in Bulgarian")]
        public string ModelBg { get; set; } = null!;

        [Required]
        [Comment("Ball Price")]
        public decimal Price { get; set; }

        [Required]
        [Comment("Available in stock")]
        public int Quantity { get; set; }

        [Required]
        [Comment("Ball Image")]
        public string ImageUrl { get; set; } = null!;

        public virtual ICollection<RacketCart> RacketCarts { get; set; } = new HashSet<RacketCart>();
    }
}
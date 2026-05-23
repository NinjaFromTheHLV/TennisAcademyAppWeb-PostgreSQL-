using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    [Comment("Bags Shop")]
    public class Bag
    {
        [Key]
        [Comment("Bag Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Bag Brand in English")]
        public string Brand { get; set; } = null!;

        [Required]
        [Comment("Bag Brand in Bulgarian")]
        public string BrandBg { get; set; } = null!;

        [Required]
        [Comment("Bag Model in English")]
        public string Model { get; set; } = null!;

        [Required]
        [Comment("Bag Model in Bulgarian")]
        public string ModelBg { get; set; } = null!;

        [Required]
        [Comment("Bag Price")]
        public decimal Price { get; set; }

        [Required]
        [Comment("Available in stock")]
        public int Quantity { get; set; }

        [Required]
        [Comment("Bag Image")]
        public string ImageUrl { get; set; } = null!;

        public virtual ICollection<BagCart> BagCarts { get; set; } = new HashSet<BagCart>();
    }
}
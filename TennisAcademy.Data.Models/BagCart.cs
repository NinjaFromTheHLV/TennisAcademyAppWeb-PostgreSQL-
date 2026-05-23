using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    public class BagCart
    {
        [Required]
        [Comment("Foreign Key of Bag")]
        public int BagId { get; set; }
        public virtual Bag Bag { get; set; } = null!;

        [Required]
        [Comment("Foreign Key of IdentityUser")]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        [Comment("Quantity of Bags in Cart")]
        public int Quantity { get; set; }
        public bool IsOrdered { get; set; } = false;
        public bool IsGift { get; set; } = false;
    }
}

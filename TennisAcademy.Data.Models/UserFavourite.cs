using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    [Comment("Users Favourite Coach")]
    public class UserFavourite
    {
        [Comment("Foreign Key which references to IdentityUser")]
        [Required]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        [Comment("Foreign Key which references to Coach")]
        [Required]
        public int CoachId { get; set; }
        public virtual Coach Coach { get; set; } = null!;
    }
}

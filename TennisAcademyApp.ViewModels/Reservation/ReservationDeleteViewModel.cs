using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.ViewModels.Reservation
{
    public class ReservationDeleteViewModel
    {
        [Required]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SurfaceName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}

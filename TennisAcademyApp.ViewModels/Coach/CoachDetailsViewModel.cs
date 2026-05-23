using TennisAcademyApp.ViewModels.Reservation;

namespace TennisAcademyApp.ViewModels.Coach
{
    public class CoachDetailsViewModel
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; } = null!;
        public string CoachNameBg { get; set; }
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = null!;
        public string DescriptionBg { get; set; }
        public int CoachAge { get; set; }
        public string Nationality { get; set; } = null!;
        public string NationalityBg { get; set; }
        public bool IsInUserFavorites { get; set; }
        public IEnumerable<ReservationIndexViewModel> CoachReservations { get; set; } = new List<ReservationIndexViewModel>();

    }
}

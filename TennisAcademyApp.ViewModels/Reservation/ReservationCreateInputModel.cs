using System.ComponentModel.DataAnnotations;
using TennisAcademyApp.GCommon.Validations;
using TennisAcademyApp.ViewModels.DropDown;

namespace TennisAcademyApp.ViewModels.Reservation
{
    public class ReservationCreateInputModel
    {
        [RequiredLocalized(nameof(RequiredMessages.Reservation.RequiredDateMessage), typeof(RequiredMessages.Reservation))]
        [Display(Name = "Select Date & Time")]
        public DateTime Date { get; set; }

        [Required]
        public int Duration { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Reservation.RequiredSurfaceMessage), typeof(RequiredMessages.Reservation))]
        [Display(Name = "Select a surface")]
        public int SurfaceId { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Reservation.RequiredTrainingTypeMessage), typeof(RequiredMessages.Reservation))]
        [Display(Name = "Select a training type")]
        public int TrainingTypeId { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Reservation.RequiredCoachMessage), typeof(RequiredMessages.Reservation))]
        [Display(Name = "Select a coach")]
        public int CoachId { get; set; }

        public string? Note { get; set; }

        public IEnumerable<CoachDropDownModel> Coaches { get; set; } = new List<CoachDropDownModel>();
        public IEnumerable<SurfaceDropDownModel> Surfaces { get; set; } = new List<SurfaceDropDownModel>();
        public IEnumerable<TrainingTypeDropDownModel> TrainingTypes { get; set; } = new List<TrainingTypeDropDownModel>();
    }
}
using System.ComponentModel.DataAnnotations;
using TennisAcademyApp.GCommon.Validations;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants.Coach;

namespace TennisAcademyApp.ViewModels.Coach
{
    public class CoachEditInputModel
    {
        public int CoachId { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Coach.RequiredNameMessage), typeof(RequiredMessages.Coach))]
        [Display(Name = "Coach Name")]
        public string Name { get; set; } = null!;

        [RequiredLocalized(nameof(RequiredMessages.Coach.AgeRequiredMessage), typeof(RequiredMessages.Coach))]
        [RangeLocalized(CoachAgeMinRequirement, CoachAgeMaxRequirement, nameof(ErrorMessages.Coach.AgeErrorMessage), typeof(ErrorMessages.Coach))]
        [Display(Name = "Coach Age")]
        public int Age { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Coach.DescriptionRequiredMessage), typeof(RequiredMessages.Coach))]
        [StringLengthLocalized(CoachDescriptionMaxLenght, nameof(ErrorMessages.Coach.DescriptionMinLengthMessage), typeof(ErrorMessages.Coach), MinimumLength = CoachDescriptionMinLenght)]
        [Display(Name = "Coach Description")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Nationality is required!")]
        [Display(Name = "Coach Nationality")]
        public string Nationality { get; set; } = null!;

        [Display(Name = "Coach Image")]
        public string? ImageUrl { get; set; }
    }
}
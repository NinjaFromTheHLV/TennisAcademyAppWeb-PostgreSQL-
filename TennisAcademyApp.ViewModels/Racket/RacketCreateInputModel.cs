using System.ComponentModel.DataAnnotations;
using TennisAcademyApp.GCommon.Validations; // Добавяме узинга за custom атрибутите

namespace TennisAcademyApp.ViewModels.Racket
{
    public class RacketCreateInputModel
    {
        [RequiredLocalized(nameof(RequiredMessages.Racket.BrandRequiredErrorMessage), typeof(RequiredMessages.Racket))]
        public string Brand { get; set; } = null!;

        [RequiredLocalized(nameof(RequiredMessages.Racket.ModelRequiredErrorMessage), typeof(RequiredMessages.Racket))]
        public string Model { get; set; } = null!;

        [RequiredLocalized(nameof(RequiredMessages.Racket.PriceRequiredErrorMessage), typeof(RequiredMessages.Racket))]
        [RangeLocalized(30.00, 1500.00, nameof(ErrorMessages.Racket.PriceRangeErrorMessage), typeof(ErrorMessages.Racket))]
        public decimal Price { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Racket.QuantityRequiredErrorMessage), typeof(RequiredMessages.Racket))]
        [RangeLocalized(1, int.MaxValue, nameof(ErrorMessages.Racket.QuantityRangeErrorMessage), typeof(ErrorMessages.Racket))]
        public int Quantity { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Racket.ImageUrlRequiredErrorMessage), typeof(RequiredMessages.Racket))]
        public string ImageUrl { get; set; } = null!;
    }
}
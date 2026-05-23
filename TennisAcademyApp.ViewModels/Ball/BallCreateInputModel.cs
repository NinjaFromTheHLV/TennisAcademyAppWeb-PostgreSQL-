using System.ComponentModel.DataAnnotations;
using TennisAcademyApp.GCommon.Validations; // Добавяме узинга за custom атрибутите

namespace TennisAcademyApp.ViewModels.Ball
{
    public class BallCreateInputModel
    {
        [RequiredLocalized(nameof(RequiredMessages.Ball.BrandRequiredErrorMessage), typeof(RequiredMessages.Ball))]
        public string Brand { get; set; } = null!;

        [RequiredLocalized(nameof(RequiredMessages.Ball.ModelRequiredErrorMessage), typeof(RequiredMessages.Ball))]
        public string Model { get; set; } = null!;

        [RequiredLocalized(nameof(RequiredMessages.Ball.PriceRequiredErrorMessage), typeof(RequiredMessages.Ball))]
        [RangeLocalized(17.00, 80.00, nameof(ErrorMessages.Ball.PriceRangeErrorMessage), typeof(ErrorMessages.Ball))]
        public decimal Price { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Ball.QuantityRequiredErrorMessage), typeof(RequiredMessages.Ball))]
        [RangeLocalized(1, int.MaxValue, nameof(ErrorMessages.Ball.QuantityRangeErrorMessage), typeof(ErrorMessages.Ball))]
        public int Quantity { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Ball.ImageUrlRequiredErrorMessage), typeof(RequiredMessages.Ball))]
        public string ImageUrl { get; set; } = null!;
    }
}
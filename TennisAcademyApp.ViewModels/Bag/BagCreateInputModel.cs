using System.ComponentModel.DataAnnotations;
using TennisAcademyApp.GCommon.Validations; // Добави този using

namespace TennisAcademyApp.ViewModels.Bag
{
    public class BagCreateInputModel
    {
        [RequiredLocalized(nameof(RequiredMessages.Bag.BrandRequiredErrorMessage), typeof(RequiredMessages.Bag))]
        public string Brand { get; set; } = null!;

        [RequiredLocalized(nameof(RequiredMessages.Bag.ModelRequiredErrorMessage), typeof(RequiredMessages.Bag))]
        public string Model { get; set; } = null!;

        [RequiredLocalized(nameof(RequiredMessages.Bag.PriceRequiredErrorMessage), typeof(RequiredMessages.Bag))]
        [RangeLocalized(50.00, 1000.00, nameof(ErrorMessages.Bag.PriceRangeErrorMessage), typeof(ErrorMessages.Bag))]
        public decimal Price { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Bag.QuantityRequiredErrorMessage), typeof(RequiredMessages.Bag))]
        [RangeLocalized(1, int.MaxValue, nameof(ErrorMessages.Bag.QuantityErrorMessage), typeof(ErrorMessages.Bag))]
        public int Quantity { get; set; }

        [RequiredLocalized(nameof(RequiredMessages.Bag.ImageUrlRequiredErrorMessage), typeof(RequiredMessages.Bag))]
        public string ImageUrl { get; set; } = null!;
    }
}
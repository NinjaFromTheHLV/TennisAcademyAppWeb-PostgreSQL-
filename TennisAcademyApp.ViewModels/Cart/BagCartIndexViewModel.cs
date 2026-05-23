using TennisAcademyApp.ViewModels.Bag;

namespace TennisAcademyApp.ViewModels.Cart
{
    public class BagCartIndexViewModel : BagIndexViewModel
    {
        public decimal TotalPrice { get; set; }
        public bool IsGift { get; set; }
    }
}

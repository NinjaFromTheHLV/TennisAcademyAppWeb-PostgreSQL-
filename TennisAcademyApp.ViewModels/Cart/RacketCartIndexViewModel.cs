using TennisAcademyApp.ViewModels.Racket;

namespace TennisAcademyApp.ViewModels.Cart
{
    public class RacketCartIndexViewModel : RacketIndexViewModel
    {
        public decimal TotalPrice { get; set; }
        public bool IsGift { get; set; }
    }
}

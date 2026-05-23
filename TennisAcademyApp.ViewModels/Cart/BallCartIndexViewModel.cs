using TennisAcademyApp.ViewModels.Ball;

namespace TennisAcademyApp.ViewModels.Cart
{
    public class BallCartIndexViewModel : BallIndexViewModel
    {
        public decimal TotalPrice { get; set; }
        public bool IsGift { get; set; }
    }
}

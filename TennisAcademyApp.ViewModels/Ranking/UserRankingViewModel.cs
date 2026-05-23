namespace TennisAcademyApp.ViewModels.Ranking
{
    public class UserRankingViewModel
    {
        public int Position { get; set; }
        public string FullName { get; set; } = null!;
        public int TournamentsCount { get; set; }
        public int ReservationsCount { get; set; }
        public int BoughtItemsCount { get; set; }

        public int TotalPoints => (TournamentsCount * 25) + (ReservationsCount * 10) + (BoughtItemsCount * 15);

        public int DiscountPercentage
        {
            get
            {
                return Position switch
                {
                    1 => 20,
                    2 => 15,
                    3 => 10,
                    _ => 0
                };
            }
        }

        public decimal GetDiscountMultiplier()
        {
            return Position switch
            {
                1 => 0.80m,
                2 => 0.85m,
                3 => 0.90m,
                _ => 1.00m 
            };
        }
    }
}
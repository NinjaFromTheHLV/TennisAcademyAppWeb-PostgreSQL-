namespace TennisAcademyApp.ViewModels.Tournament
{
    public class TournamentAllViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string TitleBg { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EntryFee { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryNameBg { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DescriptionBg { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipantsCount { get; set; }
    }
}
namespace TennisAcademyApp.ViewModels.Tournament
{
    public class TournamentDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string TitleBg { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DescriptionBg { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EntryFee { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipantsCount { get; set; }
        public bool IsAlreadyEnrolled { get; set; }
    }
}
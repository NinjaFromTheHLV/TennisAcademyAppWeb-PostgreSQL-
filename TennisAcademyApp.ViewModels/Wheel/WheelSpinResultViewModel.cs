namespace TennisAcademyApp.ViewModels.Wheel
{
    public class WheelSpinResultViewModel
    {
        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public int WinningIndex { get; set; }

        public string? Prize { get; set; }
    }
}
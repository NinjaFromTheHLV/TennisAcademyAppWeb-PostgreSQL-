namespace TennisAcademyApp.GCommon.Validations
{
    public static class ValidationConstants
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Trainer = "Coach";
        public static class Coach
        {
            public const int CoachAgeMinRequirement = 30;
            public const int CoachAgeMaxRequirement = 75;

            public const int CoachDescriptionMinLenght = 10;
            public const int CoachDescriptionMaxLenght = 150;

            public const string NoImageUrl = "~/pictures/DefaultUserImage.webp";
        }
        public static class Reservation
        {
            public const int PlayerNotesMaxLenght = 70;

            public const string DateFormat = "dd-MM-yyyy HH:mm";
        }
    }
}

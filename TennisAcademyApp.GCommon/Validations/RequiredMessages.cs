namespace TennisAcademyApp.GCommon.Validations
{
    public static class RequiredMessages
    {
        private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

        public static class Coach
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string RequiredNameMessage => IsBg ? "Името е задължително." : "Name is required.";
            public static string DescriptionRequiredMessage => IsBg ? "Описанието е задължително." : "Description is required.";
            public static string AgeRequiredMessage => IsBg ? "Възрастта е задължителна." : "Age is required.";
        }

        public static class Reservation
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string RequiredDateMessage => IsBg ? "Моля, изберете дата и час." : "Please select Date & Time.";
            public static string RequiredCoachMessage => IsBg ? "Моля, изберете треньор." : "Please select a coach.";
            public static string RequiredSurfaceMessage => IsBg ? "Моля, изберете настилка." : "Please select a surface.";
            public static string RequiredTrainingTypeMessage => IsBg ? "Моля, изберете тип тренировка." : "Please select a training type.";
        }

        public static class Racket
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string BrandRequiredErrorMessage => IsBg ? "Марката е задължителна." : "Brand is required.";
            public static string ModelRequiredErrorMessage => IsBg ? "Моделът е задължителен." : "Model is required.";
            public static string PriceRequiredErrorMessage => IsBg ? "Цената е задължителна." : "Price is required.";
            public static string QuantityRequiredErrorMessage => IsBg ? "Количеството е задължително." : "Quantity is required.";
            public static string ImageUrlRequiredErrorMessage => IsBg ? "URL адресът на изображението е задължителен." : "Image URL is required.";
            public static string RacketNotFoundErrorMessage => IsBg ? "Ракетата не е намерена." : "Racket not found.";
        }

        public static class Ball
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string BrandRequiredErrorMessage => IsBg ? "Марката е задължителна." : "Brand is required.";
            public static string ModelRequiredErrorMessage => IsBg ? "Моделът е задължителен." : "Model is required.";
            public static string PriceRequiredErrorMessage => IsBg ? "Цената е задължителна." : "Price is required.";
            public static string QuantityRequiredErrorMessage => IsBg ? "Количеството е задължително." : "Quantity is required.";
            public static string ImageUrlRequiredErrorMessage => IsBg ? "URL адресът на изображението е задължителен." : "Image URL is required.";
            public static string BallNotFoundErrorMessage => IsBg ? "Топките не са намерени." : "Ball not found.";
        }

        public static class Bag
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string BrandRequiredErrorMessage => IsBg ? "Марката е задължителна." : "Brand is required.";
            public static string ModelRequiredErrorMessage => IsBg ? "Моделът е задължителен." : "Model is required.";
            public static string PriceRequiredErrorMessage => IsBg ? "Цената е задължителна." : "Price is required.";
            public static string QuantityRequiredErrorMessage => IsBg ? "Количеството е задължително." : "Quantity is required.";
            public static string ImageUrlRequiredErrorMessage => IsBg ? "URL адресът на изображението е задължителен." : "Image URL is required.";
        }
    }
}
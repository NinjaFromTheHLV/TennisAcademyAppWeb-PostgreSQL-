namespace TennisAcademyApp.GCommon.Validations
{
    public static class SuccessfulMessages
    {
        private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

        public static class Coach
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string CoachAddedSuccessfully => IsBg ? "Треньорът беше добавен успешно." : "Coach added successfully.";
            public static string CoachUpdatedSuccessfully => IsBg ? "Треньорът беше обновен успешно." : "Coach updated successfully.";
            public static string CoachDeletedSuccessfully => IsBg ? "Треньорът беше изтрит успешно." : "Coach deleted successfully.";
            public static string CoachFavouriteAddedSuccessfully => IsBg ? "Треньорът беше добавен в любими успешно." : "Coach added to favourites successfully.";
            public static string CoachFavouriteRemovedSuccessfully => IsBg ? "Треньорът беше премахнат от любими успешно." : "Coach removed from favourites successfully.";
        }

        public static class Reservation
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string ReservationCreatedSuccessfully => IsBg ? "Резервацията беше създадена успешно." : "Reservation created successfully.";
            public static string ReservationDeletedSuccessfully => IsBg ? "Резервацията беше изтрита успешно." : "Reservation deleted successfully.";
        }

        public static class Racket
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string RacketAddedSuccessfully => IsBg ? "Ракетата беше добавена успешно." : "Racket added successfully.";
            public static string RacketUpdatedSuccessfully => IsBg ? "Ракетата беше обновена успешно." : "Racket updated successfully.";
            public static string RacketDeletedSuccessfully => IsBg ? "Ракетата беше изтрита успешно." : "Racket deleted successfully.";
        }

        public static class RacketCart
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string RacketAddedToCartSuccessfully => IsBg ? "Ракетата беше добавена в количката успешно." : "Racket added to cart successfully.";
            public static string RacketRemovedFromCartSuccessfully => IsBg ? "Ракетата беше премахната от количката успешно." : "Racket removed from cart successfully.";
            public static string RacketCheckoutSuccessful => IsBg ? "Поръчката на ракетата е успешна. Благодарим ви за покупката!" : "Racket checkout successful. Thank you for your purchase!";
        }

        public static class Ball
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string BallAddedSuccessfully => IsBg ? "Топките бяха добавени успешно." : "Ball added successfully.";
            public static string BallUpdatedSuccessfully => IsBg ? "Топките бяха обновени успешно." : "Ball updated successfully.";
            public static string BallDeletedSuccessfully => IsBg ? "Топките бяха изтрити успешно." : "Ball deleted successfully.";
        }

        public static class BallCart
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string BallAddedToCartSuccessfully => IsBg ? "Топките бяха добавени в количката успешно." : "Ball added to cart successfully.";
            public static string BallRemovedFromCartSuccessfully => IsBg ? "Топките бяха премахнати от количката успешно." : "Ball removed from cart successfully.";
            public static string BallCheckoutSuccessful => IsBg ? "Поръчката на топките е успешна. Благодарим ви за покупката!" : "Ball checkout successful. Thank you for your purchase!";
        }

        public static class Bag
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string BagAddedSuccessfully => IsBg ? "Чантата беше добавена успешно." : "Bag added successfully.";
            public static string BagUpdatedSuccessfully => IsBg ? "Чантата беше обновена успешно." : "Bag updated successfully.";
            public static string BagDeletedSuccessfully => IsBg ? "Чантата беше изтрита успешно." : "Bag deleted successfully.";
        }

        public static class BagCart
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string BagAddedToCartSuccessfully => IsBg ? "Чантата беше добавена в количката успешно." : "Bag added to cart successfully.";
            public static string BagRemovedFromCartSuccessfully => IsBg ? "Чантата беше премахната от количката успешно." : "Bag removed from cart successfully.";
            public static string BagCheckoutSuccessful => IsBg ? "Поръчката на чантата е успешна. Благодарим ви за покупката!" : "Bag checkout successful. Thank you for your purchase!";
        }

        public static class UserManagement
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string UserAssignedToRoleSuccessfully => IsBg ? "Ролята беше назначена на потребителя успешно." : "User assigned to role successfully.";
            public static string UserRemovedFromRoleSuccessfully => IsBg ? "Ролята беше премахната от потребителя успешно." : "User removed from role successfully.";
            public static string UserRemovedSuccessfully => IsBg ? "Потребителят беше премахнат успешно." : "User removed successfully.";
        }
        public static class Tournament
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string TournamentEnrollSuccess => IsBg ? "Успешно се записахте за турнира!" : "Successfully joined the tournament!";
            public static string TournamentUnenrollSuccess => IsBg ? "Успешно се отписахте от турнира." : "Successfully left the tournament.";
        }
    }
}
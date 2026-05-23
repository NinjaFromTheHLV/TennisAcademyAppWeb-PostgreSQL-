namespace TennisAcademyApp.GCommon.Validations
{
    public static class ErrorMessages
    {
        private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

        public static string InvalidData => IsBg
            ? "Невалидни данни, моля опитайте отново."
            : "Invalid data, please try again";

        public static string UnexpectedError => IsBg
            ? "Възникна неочаквана грешка, моля опитайте отново по-късно."
            : "An unexpected error occurred, please try again later.";

        public static class Reservation
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string PastDateErrorMessage => IsBg ? "Моля, изберете валидна дата." : "Please select a valid date.";
            public static string TwoHoursErrorMessage => IsBg ? "Резервации могат да се правят най-малко два часа от текущото време." : "Reservations can be made at least two hours from now.";
            public static string SelectedTimeErrorMessage => IsBg ? "Работното време на Академията е от 8:00 до 20:00." : "The Academy's work time is from 8:00 to 20:00.";
            public static string FutureDateErrorMessage => IsBg ? "Резервации могат да се правят за следващите 60 дни." : "Reservations can be made for the next 60 days.";
            public static string SundayErrorMessage => IsBg ? "Неделя е почивен ден! Моля, изберете друго време." : "Sunday is off day! Please choose other time.";
            public static string DurationErrorMessage => IsBg ? "Продължителността трябва да бъде 60 или 120 минути." : "Duration must be either 60 or 120 minutes.";
            public static string CoachNotAvailableErrorMessage => IsBg ? "Избраният треньор не е свободен в избраното време." : "The selected coach is not available at the chosen time.";
            public static string ReservationNotFoundErrorMessage => IsBg ? "Резервацията не е намерена." : "Reservation not found.";
            public static string ReservationDeleteErrorMessage => IsBg ? "Възникна грешка при изтриването на резервацията, опитайте отново." : "An error occurred while deleting the reservation, try again.";
        }

        public static class Coach
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string AgeErrorMessage => IsBg ? "Възрастта трябва да бъде между 30 & 75 години!" : "Age must be between 30 and 75!";
            public static string DescriptionMinLengthMessage => IsBg ? "Описанието трябва да бъде поне 10 символа." : "Description must be at least 10 characters.";
            public static string DescriptionMaxLengthMessage => IsBg ? "Описанието не може да надвишава 150 символа." : "Description cannot exceed 150 characters.";
            public static string CoachNotFoundErrorMessage => IsBg ? "Треньорът не е намерен." : "Coach not found.";
            public static string CoachCannotBeNullErrorMessage => IsBg ? "Треньорът не може да бъде празен." : "Coach cannot be null.";
            public static string CoachAddErrorMessage => IsBg ? "Възникна грешка при добавянето на треньора, опитайте отново." : "An error occured while adding a coach, try again.";
            public static string CoachEditErrorMessage => IsBg ? "Възникна грешка при редакцията на треньора, опитайте отново." : "An error occured while editing a coach, try again.";
            public static string CoachDeleteErrorMessage => IsBg ? "Възникна грешка при изтриването на треньора, oпитайте отново." : "An error occured while deleting a coach, try again.";
            public static string CoachAlreadyAddedToFavouritesErrorMessage => IsBg ? "Треньорът вече е добавен в любими." : "Coach already added to favourites.";
        }

        public static class Racket
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string RacketNotFoundErrorMessage => IsBg ? "Ракетата не е намерена." : "Racket not found.";
            public static string RacketCannotBeNullErrorMessage => IsBg ? "Ракетата не може да бъде празна." : "Racket cannot be null.";
            public static string PriceRangeErrorMessage => IsBg ? "Цената трябва да бъде между 30 и 1500." : "Price must be between 30 and 1500.";
            public static string QuantityRangeErrorMessage => IsBg ? "Количеството трябва да бъде положително число." : "Quantity must be a positive number.";
            public static string RacketAddErrorMessage => IsBg ? "Възникна грешка при добавянето на ракетата, опитайте отново." : "An error occurred while adding the racket, try again.";
            public static string RacketEditErrorMessage => IsBg ? "Възникна грешка при редакцията на ракетата, опитайте отново." : "An error occurred while editing the racket, try again.";
            public static string RacketDeleteErrorMessage => IsBg ? "Възникна грешка при изтриването на ракетата, опитайте отново." : "An error occurred while deleting the racket, try again.";
        }

        public static class User
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string UserCannotBeNull => IsBg ? "Моля, влезте в профила си и опитайте отново." : "Please log in and try again.";
        }

        public static class RacketCart
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string InvalidQuantityErrorMessage => IsBg ? "Невалидно количество. Моля, въведете валидно число." : "Invalid quantity. Please enter a valid number.";
            public static string RacketFailedToRemoveFromCartErrorMessage => IsBg ? "Възникна грешка при премахването на ракетата от количката, опитайте отново." : "An error occurred while removing the racket from the cart, try again.";
            public static string UnableToCheckoutErrorMessage => IsBg ? "Не можете да завършите поръчка с празна количка. Моля, първо добавете ракета." : "You cannot checkout an empty cart. Please add a racket to the cart first.";
            public static string CannotLoadRacketCartErrorMessage => IsBg ? "Възникна грешка при зареждането на количката с ракети, опитайте отново." : "An error occurred while loading the racket cart, try again.";
            public static string RacketNotFoundInCartErrorMessage => IsBg ? "Ракетата не е намерена в количката." : "Racket not found in cart.";
        }

        public static class Ball
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string BallNotFoundErrorMessage => IsBg ? "Топките не са намерени." : "Ball not found.";
            public static string BallCannotBeNullErrorMessage => IsBg ? "Топките не могат да бъдат празни." : "Ball cannot be null.";
            public static string PriceRangeErrorMessage => IsBg ? "Цената трябва да бъде между 17 и 80." : "Price must be between 17 and 80.";
            public static string QuantityRangeErrorMessage => IsBg ? "Количеството трябва да бъде положително число." : "Quantity must be a positive number.";
            public static string BallAddErrorMessage => IsBg ? "Възникна грешка при добавянето на топките, опитайте отново." : "An error occurred while adding the ball, try again.";
            public static string BallEditErrorMessage => IsBg ? "Възникна грешка при редакцията на топките, опитайте отново." : "An error occurred while editing the ball, try again.";
            public static string BallDeleteErrorMessage => IsBg ? "Възникна грешка при изтриването на топките, опитайте отново." : "An error occurred while deleting the ball, try again.";
        }

        public static class BallCart
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string InvalidQuantityErrorMessage => IsBg ? "Невалидно количество. Моля, въведете валидно число." : "Invalid quantity. Please enter a valid number.";
            public static string BallFailedToRemoveFromCartErrorMessage => IsBg ? "Възникна грешка при премахването на топките от количката, опитайте отново." : "An error occurred while removing the ball from the cart, try again.";
            public static string UnableToCheckoutErrorMessage => IsBg ? "Не можете да завършите поръчка с празна количка. Моля, първо добавете топки." : "You cannot checkout an empty cart. Please add a ball to the cart first.";
            public static string CannotLoadBallCartErrorMessage => IsBg ? "Възникна грешка при зареждането на количката с топки, опитайте отново." : "An error occurred while loading the ball cart, try again.";
            public static string BallNotFoundInCartErrorMessage => IsBg ? "Топките не са намерени в количката." : "Ball not found in cart.";
        }

        public static class Bag
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string PriceRangeErrorMessage => IsBg ? "Цената трябва да бъде между 50.00 и 1000.00." : "Price must be between 50.00 and 1000.00.";
            public static string QuantityErrorMessage => IsBg ? "Количеството трябва да бъде положително число." : "Quality must be a positive number";
            public static string BagNotFoundErrorMessage => IsBg ? "Чантата не е намерена." : "Bag not found.";
            public static string BagCannotBeNullErrorMessage => IsBg ? "Чантата не може да бъде празна." : "Bag cannot be null.";
            public static string BagAddErrorMessage => IsBg ? "Възникна грешка при добавянето на чантата, опитайте отново." : "An error occurred while adding the bag, try again.";
            public static string BagEditErrorMessage => IsBg ? "Възникна грешка при редакцията на чантата, опитайте отново." : "An error occurred while editing the bag, try again.";
            public static string BagDeleteErrorMessage => IsBg ? "Възникна грешка при изтриването на чантата, опитайте отново." : "An error occurred while deleting the bag, try again.";
        }

        public static class BagCart
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string InvalidQuantityErrorMessage => IsBg ? "Невалидно количество. Моля, въведете валидно число." : "Invalid quantity. Please enter a valid number.";
            public static string BagFailedToRemoveFromCartErrorMessage => IsBg ? "Възникна грешка при премахването на чантата от количката, опитайте отново." : "An error occurred while removing the bag from the cart, try again.";
            public static string UnableToCheckoutErrorMessage => IsBg ? "Не можете да завършите поръчка с празна количка. Моля, първо добавете чанта." : "You cannot checkout an empty cart. Please add a bag to the cart first.";
            public static string CannotLoadBagCartErrorMessage => IsBg ? "Възникна грешка при зареждането на количката с чанти, опитайте отново." : "An error occurred while loading the bag cart, try again.";
            public static string BagNotFoundInCartErrorMessage => IsBg ? "Чантата не е намерена в количката." : "Bag not found in cart.";
        }

        public static class UserManagement
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string UserAlreadyInRoleErrorMessage => IsBg ? "Потребителят вече притежава тази роля." : "User is already in the specified role.";
            public static string UserNotInRoleErrorMessage => IsBg ? "Потребителят не притежава тази роля." : "User is not in the specified role.";
            public static string UserFailedToRemoveFromRoleErrorMessage => IsBg ? "Възникна грешка при премахването на потребителя от ролята, опитайте отново." : "An error occurred while removing the user from the role, try again.";
        }
        public static class Tournament
        {
            private static bool IsBg => System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "bg";

            public static string TournamentEnrollError => IsBg ? "Неуспешно записване. Може би турнирът е пълен или вече сте записан." : "Failed to join. The tournament might be full or you are already enrolled.";
            public static string TournamentUnenrollError => IsBg ? "Неуспешно отписване. Може би вече не сте част от този турнир." : "Failed to leave. You might not be enrolled in this tournament.";
        }
    }
}
namespace TennisAcademyApp.GCommon.TextUtility
{
    public static class TextUtility
    {
        private static readonly Dictionary<string, string> LatinToCyrillic = new Dictionary<string, string>
        {
            {"Ch", "Ч"}, {"Ch'", "Ч"}, {"Sh", "Ш"}, {"Shch", "Щ"}, {"Zh", "Ж"}, {"Ya", "Я"}, {"Yu", "Ю"}, {"Yu'", "Ю"}, {"Ye", "Е"},
            {"ch", "ч"}, {"sh", "ш"}, {"shch", "щ"}, {"zh", "ж"}, {"ya", "я"}, {"yu", "ю"}, {"ye", "е"},
            {"A", "А"}, {"B", "Б"}, {"V", "В"}, {"G", "Г"}, {"D", "Д"}, {"E", "Е"}, {"Z", "З"}, {"I", "И"}, {"J", "Й"},
            {"K", "К"}, {"L", "Л"}, {"M", "М"}, {"N", "Н"}, {"O", "О"}, {"P", "П"}, {"R", "Р"}, {"S", "С"}, {"T", "Т"},
            {"U", "У"}, {"F", "Ф"}, {"H", "Х"}, {"C", "Ц"}, {"Y", "Й"}, {"X", "Х"}, {"W", "В"}, {"Q", "К"},
            {"a", "а"}, {"b", "б"}, {"v", "в"}, {"g", "г"}, {"d", "д"}, {"e", "е"}, {"z", "з"}, {"i", "и"}, {"j", "й"},
            {"k", "к"}, {"l", "л"}, {"m", "м"}, {"n", "н"}, {"o", "о"}, {"p", "п"}, {"r", "р"}, {"s", "с"}, {"t", "т"},
            {"u", "у"}, {"f", "ф"}, {"h", "х"}, {"c", "ц"}, {"y", "й"}, {"x", "х"}, {"w", "в"}, {"q", "к"}
        };

        public static string TransliterateToBg(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            foreach (var item in LatinToCyrillic)
            {
                text = text.Replace(item.Key, item.Value);
            }
            return text;
        }

        public static string Capitalize(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "Неизвестна";
            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }

        public static string CapitalizeNames(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return fullName;

            return string.Join(" ", fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(name => Capitalize(name)));
        }
    }
}
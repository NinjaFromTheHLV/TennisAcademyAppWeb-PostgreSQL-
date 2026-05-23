using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Seeding
{
    public class CoachSeeding : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> config)
        {
            config.HasData(
                new Coach
                {
                    CoachId = 1,
                    Name = "Rafael Nadal",
                    NameBg = "Рафаел Надал",
                    Age = 38,
                    Description = "One of the greatest tennis players of all time, known for his clay court dominance.",
                    DescriptionBg = "Един от най-великите тенисисти на всички времена, известен с доминацията си на клей кортове.",
                    Nationality = "Spanish",
                    NationalityBg = "Испанец",
                    ImageUrl = "https://imageio.forbes.com/specials-images/imageserve/5ece8a5c938ec500060aae37/0x0.jpg?format=jpg&crop=2462,2460,x503,y156,safe&height=416&width=416&fit=bounds"
                },
                new Coach
                {
                    CoachId = 2,
                    Name = "Roger Federer",
                    NameBg = "Роджър Федерер",
                    Age = 43,
                    Description = "Swiss tennis legend with unmatched elegance and 20 Grand Slam titles.",
                    DescriptionBg = "Швейцарска тенис легенда с ненадмината елегантност и 20 титли от Големия шлем.",
                    Nationality = "Swiss",
                    NationalityBg = "Швейцарец",
                    ImageUrl = "https://a.espncdn.com/combiner/i?img=/i/headshots/tennis/players/full/425.png"
                },
                new Coach
                {
                    CoachId = 3,
                    Name = "Novak Djokovic",
                    NameBg = "Новак Джокович",
                    Age = 37,
                    Description = "Serbian champion, known for his resilience and complete game.",
                    DescriptionBg = "Сръбски шампион, известен със своята издръжливост и комплексна игра.",
                    Nationality = "Serbian",
                    NationalityBg = "Сърбин",
                    ImageUrl = "https://a.espncdn.com/i/headshots/tennis/players/full/296.png"
                },
                new Coach
                {
                    CoachId = 4,
                    Name = "Andre Agassi",
                    NameBg = "Андре Агаси",
                    Age = 55,
                    Description = "American icon who redefined tennis in the 90s with a colorful personality.",
                    DescriptionBg = "Американска икона, която предефинира тениса през 90-те години с колоритна идентичност.",
                    Nationality = "American",
                    NationalityBg = "Американец",
                    ImageUrl = "https://www.atptour.com/-/media/alias/player-headshot/A092"
                },
                new Coach
                {
                    CoachId = 5,
                    Name = "Björn Borg",
                    NameBg = "Бьорн Борг",
                    Age = 68,
                    Description = "Swedish legend with ice-cold nerves and six French Open titles.",
                    DescriptionBg = "Шведска легенда с ледени нерви и шест титли от Ролан Гарос.",
                    Nationality = "Swedish",
                    NationalityBg = "Швед",
                    ImageUrl = "https://lavercup.com/wp-content/uploads/2022/12/figure-borg-2.png"
                }
             );
        }
    }
}
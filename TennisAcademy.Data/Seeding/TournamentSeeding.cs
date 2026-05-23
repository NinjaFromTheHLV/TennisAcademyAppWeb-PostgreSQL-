using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Seeding
{
    public class TournamentSeeding : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {  
            builder.HasData(
                new Tournament
                {
                    Id = 1,
                    Title = "Spring Clay Court Open",
                    TitleBg = "Пролетен отворен шампионат на клей",
                    Description = "Annual spring tournament open for all non-professional male players. Format: Direct elimination.",
                    DescriptionBg = "Годишен пролетен турнир, отворен за всички непрофесионални играчи (мъже). Формат: Директна елиминация.",
                    StartDate = new DateTime(2026, 05, 25),
                    EndDate = new DateTime(2026, 05, 31),
                    EntryFee = 40.00m,
                    MaxParticipants = 32,
                    CategoryId = 1,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 2,
                    Title = "Academy Women's Cup",
                    TitleBg = "Купа на Академията за жени",
                    Description = "Special dynamic tournament for women. Beautiful trophies and sponsor prizes provided.",
                    DescriptionBg = "Специален динамичен турнир за жени. Осигурени са красиви трофеи и награди от спонсори.",
                    StartDate = new DateTime(2026, 06, 10),
                    EndDate = new DateTime(2026, 06, 14),
                    EntryFee = 35.00m,
                    MaxParticipants = 16,
                    CategoryId = 2,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 3,
                    Title = "Junior Summer Slams",
                    TitleBg = "Младежки летен шлем",
                    Description = "Tournament targeted at young talents up to 18 years old. Great opportunity to boost local ranking points.",
                    DescriptionBg = "Турнир, насочен към млади таланти до 18 години. Страхотна възможност за трупане на точки за местната ранглиста.",
                    StartDate = new DateTime(2026, 06, 20),
                    EndDate = new DateTime(2026, 06, 25),
                    EntryFee = 20.00m,
                    MaxParticipants = 24,
                    CategoryId = 3,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 4,
                    Title = "Midsummer Mixed Doubles",
                    TitleBg = "Летни Смесени Двойки",
                    Description = "Bring your partner and fight for the grand trophy. Fun and highly competitive atmosphere.",
                    DescriptionBg = "Доведете партньора си и се борете за голямия трофей. Забавна и силно конкурентна атмосфера.",
                    StartDate = new DateTime(2026, 07, 05),
                    EndDate = new DateTime(2026, 07, 10),
                    EntryFee = 50.00m,
                    MaxParticipants = 16,
                    CategoryId = 4,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 5,
                    Title = "Masters Veterans Tournament",
                    TitleBg = "Мастърс Турнир за Ветерани",
                    Description = "Exclusively for players aged 45 and above. Hard court battles, tactical play, and great experience.",
                    DescriptionBg = "Ексклузивно за играчи на възраст 45 и повече години. Битки на твърди кортове, тактическа игра и страхотно изживяване.",
                    StartDate = new DateTime(2026, 07, 18),
                    EndDate = new DateTime(2026, 07, 23),
                    EntryFee = 45.00m,
                    MaxParticipants = 32,
                    CategoryId = 5, 
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 6,
                    Title = "Weekend Warrior Amateur League",
                    TitleBg = "Лига 'Уикенд Воини' за Аматьори",
                    Description = "Perfect tournament for recreation players who want to try competitive tennis. Matches played after 18:00.",
                    DescriptionBg = "Перфектен турнир за любители, които искат да се пробват в състезателния тенис. Мачовете се играят след 18:00 часа.",
                    StartDate = new DateTime(2026, 08, 01),
                    EndDate = new DateTime(2026, 08, 15),
                    EntryFee = 30.00m,
                    MaxParticipants = 64,
                    CategoryId = 6,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 7,
                    Title = "August Night Hardcourt Championship",
                    TitleBg = "Августовски Нощен Шампионат",
                    Description = "Experience the thrill of playing under the lights. Evening matches on fast hard courts.",
                    DescriptionBg = "Изживейте тръпката от играта под светлините на прожекторите. Вечерни мачове на бързи твърди кортове.",
                    StartDate = new DateTime(2026, 08, 20),
                    EndDate = new DateTime(2026, 08, 26),
                    EntryFee = 55.00m,
                    MaxParticipants = 32,
                    CategoryId = 1,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 8,
                    Title = "Autumn Women Single Open",
                    TitleBg = "Есенен Отворен Шампионат за Жени",
                    Description = "Gathering the best local female players for an end-of-season showdown on clay.",
                    DescriptionBg = "Събиране на най-добрите местни тенисистки за сблъсък в края на сезона на клей корт.",
                    StartDate = new DateTime(2026, 09, 12),
                    EndDate = new DateTime(2026, 09, 16),
                    EntryFee = 35.00m,
                    MaxParticipants = 16,
                    CategoryId = 2,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 9,
                    Title = "Back to School Youth Cup",
                    TitleBg = "Младежка Купа 'Обратно на Училище'",
                    Description = "An exciting singles tournament for juniors to celebrate the new school season. Lots of prizes.",
                    DescriptionBg = "Вълнуващ сингъл турнир за юноши по случай новия учебен сезон. Множество награди.",
                    StartDate = new DateTime(2026, 09, 25),
                    EndDate = new DateTime(2026, 09, 28),
                    EntryFee = 15.00m,
                    MaxParticipants = 32,
                    CategoryId = 3,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 10,
                    Title = "Golden Autumn Doubles",
                    TitleBg = "Златна Есен Смесени Двойки",
                    Description = "The ultimate team tournament before moving to indoor courts. Group phase followed by eliminations.",
                    DescriptionBg = "Финалният отборен турнир преди преместването в закрити кортове. Групова фаза, последвана от елиминации.",
                    StartDate = new DateTime(2026, 10, 10),
                    EndDate = new DateTime(2026, 10, 15),
                    EntryFee = 60.00m,
                    MaxParticipants = 16,
                    CategoryId = 4,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 11,
                    Title = "Indoor Premium Cup",
                    TitleBg = "Закрит Премиум Шампионат",
                    Description = "The first grand tournament of the winter season inside the academy’s premium heated halls.",
                    DescriptionBg = "Първият голям турнир за зимния сезон вътре в премиум отопляемите зали на академията.",
                    StartDate = new DateTime(2026, 11, 05),
                    EndDate = new DateTime(2026, 11, 10),
                    EntryFee = 50.00m,
                    MaxParticipants = 32,
                    CategoryId = 1,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 12,
                    Title = "Legends 45+ Winter Trophy",
                    TitleBg = "Зимна Трофейна Лига за Легенди 45+",
                    Description = "Winter edition of our highly anticipated veteran tournament. Keep the competitive spirit alive.",
                    DescriptionBg = "Зимно издание на нашия дългоочакван ветерански турнир. Поддържайте състезателния дух жив.",
                    StartDate = new DateTime(2026, 11, 22),
                    EndDate = new DateTime(2026, 11, 26),
                    EntryFee = 45.00m,
                    MaxParticipants = 16,
                    CategoryId = 5,
                    IsDeleted = false
                },
                new Tournament
                {
                    Id = 13,
                    Title = "Christmas Academy Charity Slams",
                    TitleBg = "Коледен Благотворителен Шлем на Академията",
                    Description = "Our final event of the year. All entry fees will be donated to local youth sports development.",
                    DescriptionBg = "Последното ни събитие за годината. Всички такси за участие ще бъдат дарени за развитието на местния младежки спорт.",
                    StartDate = new DateTime(2026, 12, 15),
                    EndDate = new DateTime(2026, 12, 20),
                    EntryFee = 40.00m,
                    MaxParticipants = 64,
                    CategoryId = 6,
                    IsDeleted = false
                }
            );
        }
    }
}
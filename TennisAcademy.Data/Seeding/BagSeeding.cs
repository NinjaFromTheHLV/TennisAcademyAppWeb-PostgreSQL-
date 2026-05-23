using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Seeding
{
    public class BagSeeding : IEntityTypeConfiguration<Bag>
    {
        public void Configure(EntityTypeBuilder<Bag> config)
        {
            config.HasData(
                new Bag
                {
                    Id = 1,
                    Brand = "Wilson",
                    BrandBg = "Уилсън",
                    Model = "Team 3-Pack",
                    ModelBg = "Тийм 3-Пак",
                    Price = 59.99m,
                    Quantity = 10,
                    ImageUrl = "https://cdn.media.amplience.net/i/sportinglife/25918789_0/Team-3-Pack-Tennis-Bag?$default$&fmt=auto&w=540&h=540"
                },
                new Bag
                {
                    Id = 2,
                    Brand = "Head",
                    BrandBg = "Хед",
                    Model = "Tour Team 6R",
                    ModelBg = "Тур Тийм 6Р",
                    Price = 89.99m,
                    Quantity = 7,
                    ImageUrl = "https://media.strefatenisa.com.pl/public/media/20/c1/2b/1721072068/head-tour-team-6r-combi-black-mixed-1.jpg?ts=1745860751"
                },
                new Bag
                {
                    Id = 3,
                    Brand = "Babolat",
                    BrandBg = "Баболат",
                    Model = "Pure Drive RHx6",
                    ModelBg = "Пюр Драйв Ер Ха х6",
                    Price = 99.99m,
                    Quantity = 5,
                    ImageUrl = "https://m.media-amazon.com/images/I/61vGrieRbCL._UF1000,1000_QL80_.jpg"
                },
                new Bag
                {
                    Id = 4,
                    Brand = "Yonex",
                    BrandBg = "Йонекс",
                    Model = "Pro Series 9-Pack",
                    ModelBg = "Про Сериес 9-Пак",
                    Price = 129.99m,
                    Quantity = 4,
                    ImageUrl = "https://www.midwestracquetsports.com/images/xl/BAG92429BK.jpg?v=1"
                }
            );
        }
    }
}
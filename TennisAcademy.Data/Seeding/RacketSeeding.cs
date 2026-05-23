using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Seeding
{
    public class RacketConfiguration : IEntityTypeConfiguration<Racket>
    {
        public void Configure(EntityTypeBuilder<Racket> config)
        {
            config.HasData(
                new Racket
                {
                    Id = 1,
                    Brand = "Wilson",
                    BrandBg = "Уилсън",
                    Model = "Pro Staff 97",
                    ModelBg = "Про Стаф 97",
                    Price = 349.99m,
                    Quantity = 5,
                    ImageUrl = "https://cdncloudcart.com/28710/products/images/134337/tenis-raketa-wilson-pro-staff-rf-97-v13-0-tns-fr-image_6358bfebb40a9_800x800.jpeg?1666760684"
                },
                new Racket
                {
                    Id = 2,
                    Brand = "Babolat",
                    BrandBg = "Баболат",
                    Model = "Pure Drive",
                    ModelBg = "Пюр Драйв",
                    Price = 299.99m,
                    Quantity = 8,
                    ImageUrl = "https://babolat.bg/image/cache/catalog/tennis/2024/rackets/101474/101474-Pure_Drive_98-136-1-Face_2-250x250.jpg"
                },
                new Racket
                {
                    Id = 3,
                    Brand = "Head",
                    BrandBg = "Хед",
                    Model = "Graphene 360+ Speed",
                    ModelBg = "Графен 360+ Спийд",
                    Price = 279.99m,
                    Quantity = 10,
                    ImageUrl = "https://i.sportisimo.com/products/images/1104/1104555/700x700/head-graphene-360-speed-mp_1.jpg"
                },
                new Racket
                {
                    Id = 4,
                    Brand = "Yonex",
                    BrandBg = "Йонекс",
                    Model = "Ezone 98",
                    ModelBg = "Езоун 98",
                    Price = 319.99m,
                    Quantity = 6,
                    ImageUrl = "https://us.yonex.com/cdn/shop/files/EZ0898_BlastBlue_5868.jpg?v=1739481973&width=1946"
                },
                new Racket
                {
                    Id = 5,
                    Brand = "Prince",
                    BrandBg = "Принс",
                    Model = "Tour 100P",
                    ModelBg = "Тур 100П",
                    Price = 259.99m,
                    Quantity = 4,
                    ImageUrl = "https://images.squarespace-cdn.com/content/v1/56e9b38c2b8dde820241b62d/1471886555425-JT9KKFKPOL4FNLAV9ZB0/r2.jpg"
                },
                new Racket
                {
                    Id = 6,
                    Brand = "Tecnifibre",
                    BrandBg = "Технифайбър",
                    Model = "TFight 305",
                    ModelBg = "Т-Файт 305",
                    Price = 289.99m,
                    Quantity = 7,
                    ImageUrl = "https://www.tecnifibre.com/dw/image/v2/BHDN_PRD/on/demandware.static/-/Sites-tecnifibre-master-catalog/default/dwcf93310b/hi-res/T-FIGHT%202025/Packshots/305S/14FI305S5_04.jpg?sw=608&sh=608&sm=fit"
                }
            );
        }
    }
}
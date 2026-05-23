using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Cart;
using TennisAcademyApp.ViewModels.Ranking;

namespace TennisAcademyApp.Tests
{
    [TestFixture]
    public class RacketCartServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private RacketCartService racketCartService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<IRankingService> mockRankingService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TennisAcademyDbContext>()
                .UseInMemoryDatabase(databaseName: "TennisAcademyTestDb_" + Guid.NewGuid().ToString())
                .Options;

            dbContext = new TennisAcademyDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            mockRankingService = new Mock<IRankingService>();

            racketCartService = new RacketCartService(dbContext, mockUserManager.Object, mockRankingService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllRacketsInCartAsync_ReturnsEmpty_WhenUserNotFound()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

            var result = await racketCartService.GetAllRacketsInCartAsync("invalid-user");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllRacketsInCartAsync_CalculatesDiscountCorrectly_ForRankOne()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId, FirstName = "Jane", LastName = "Doe" };

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var racket = new Racket
            {
                Id = 1,
                Brand = "Wilson",
                BrandBg = "Уилсън",
                Price = 200,
                Quantity = 50,
                Model = "Pro Staff",
                ModelBg = "Про Стаф",
                ImageUrl = "racket.jpg"
            };
            var cartItem = new RacketCart { UserId = userId, RacketId = 1, Quantity = 2, IsOrdered = false, IsGift = false };

            dbContext.Rackets.Add(racket);
            dbContext.RacketCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            mockRankingService.Setup(rs => rs.GetLeaderboardAsync())
                .ReturnsAsync(new List<UserRankingViewModel>
                {
                    new UserRankingViewModel { FullName = "Jane Doe", Position = 1 }
                });

            var result = await racketCartService.GetAllRacketsInCartAsync(userId);
            var item = result.First();

            Assert.That(item.Price, Is.EqualTo(160m));
            Assert.That(item.TotalPrice, Is.EqualTo(320m));
        }

        [Test]
        public async Task GetAllRacketsInCartAsync_SetsPriceToZero_WhenItemIsGift()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId, FirstName = "Jane", LastName = "Doe" };

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockRankingService.Setup(rs => rs.GetLeaderboardAsync()).ReturnsAsync(new List<UserRankingViewModel>());

            var racket = new Racket
            {
                Id = 1,
                Brand = "Head",
                BrandBg = "Хед",
                Price = 150,
                Quantity = 50,
                Model = "Radical",
                ModelBg = "Радикал",
                ImageUrl = "racket.jpg"
            };
            var cartItem = new RacketCart { UserId = userId, RacketId = 1, Quantity = 3, IsOrdered = false, IsGift = true };

            dbContext.Rackets.Add(racket);
            dbContext.RacketCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            var result = await racketCartService.GetAllRacketsInCartAsync(userId);
            var item = result.First();

            Assert.That(item.Price, Is.EqualTo(0m));
            Assert.That(item.TotalPrice, Is.EqualTo(0m));
        }

        [Test]
        public void AddRacketToCartAsync_ThrowsException_WhenRacketDoesNotExist()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await racketCartService.AddRacketToCartAsync("user1", 99, 1));
        }

        [Test]
        public async Task AddRacketToCartAsync_ThrowsException_WhenQuantityIsInvalid()
        {
            var racket = new Racket
            {
                Id = 1,
                Brand = "Babolat",
                BrandBg = "Баболат",
                Quantity = 5,
                Model = "Pure Aero",
                ModelBg = "Пюр Аеро",
                ImageUrl = "racket.jpg"
            };
            dbContext.Rackets.Add(racket);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await racketCartService.AddRacketToCartAsync("user1", 1, 0));

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await racketCartService.AddRacketToCartAsync("user1", 1, 10));
        }

        [Test]
        public async Task AddRacketToCartAsync_AddsNewItem_WhenNotInCart()
        {
            var racket = new Racket
            {
                Id = 1,
                Brand = "Yonex",
                BrandBg = "Йонекс",
                Quantity = 20,
                Model = "Ezone",
                ModelBg = "Изоун",
                ImageUrl = "racket.jpg"
            };
            dbContext.Rackets.Add(racket);
            await dbContext.SaveChangesAsync();

            var result = await racketCartService.AddRacketToCartAsync("user1", 1, 5);

            var cartItem = await dbContext.RacketCart.FirstOrDefaultAsync();
            var updatedRacket = await dbContext.Rackets.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(cartItem, Is.Not.Null);
            Assert.That(cartItem!.Quantity, Is.EqualTo(5));
            Assert.That(cartItem.IsGift, Is.False);
            Assert.That(cartItem.IsOrdered, Is.False);
            Assert.That(updatedRacket!.Quantity, Is.EqualTo(15));
        }

        [Test]
        public async Task AddRacketToCartAsync_IncreasesQuantity_WhenAlreadyInCartAndNotOrdered()
        {
            var racket = new Racket
            {
                Id = 1,
                Brand = "Head",
                BrandBg = "Хед",
                Quantity = 20,
                Model = "Speed",
                ModelBg = "Спийд",
                ImageUrl = "racket.jpg"
            };
            var existingCartItem = new RacketCart { UserId = "user1", RacketId = 1, Quantity = 2, IsOrdered = false, IsGift = false };

            dbContext.Rackets.Add(racket);
            dbContext.RacketCart.Add(existingCartItem);
            await dbContext.SaveChangesAsync();

            var result = await racketCartService.AddRacketToCartAsync("user1", 1, 3);

            var updatedCartItem = await dbContext.RacketCart.FirstAsync();
            var updatedRacket = await dbContext.Rackets.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedCartItem.Quantity, Is.EqualTo(5));
            Assert.That(updatedRacket!.Quantity, Is.EqualTo(17));
        }

        [Test]
        public async Task AddRacketToCartAsync_ResetsItem_WhenAlreadyInCartButOrdered()
        {
            var racket = new Racket
            {
                Id = 1,
                Brand = "Babolat",
                BrandBg = "Баболат",
                Quantity = 20,
                Model = "Pure Drive",
                ModelBg = "Пюр Драйв",
                ImageUrl = "racket.jpg"
            };
            var existingCartItem = new RacketCart { UserId = "user1", RacketId = 1, Quantity = 5, IsOrdered = true, IsGift = false };

            dbContext.Rackets.Add(racket);
            dbContext.RacketCart.Add(existingCartItem);
            await dbContext.SaveChangesAsync();

            var result = await racketCartService.AddRacketToCartAsync("user1", 1, 4);

            var updatedCartItem = await dbContext.RacketCart.FirstAsync();
            var updatedRacket = await dbContext.Rackets.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedCartItem.Quantity, Is.EqualTo(5));
            Assert.That(updatedCartItem.IsOrdered, Is.False);
            Assert.That(updatedRacket!.Quantity, Is.EqualTo(16));
        }

        [Test]
        public void RemoveRacketFromCartAsync_ThrowsException_WhenItemNotFound()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await racketCartService.RemoveRacketFromCartAsync("user1", 1));
        }

        [Test]
        public async Task RemoveRacketFromCartAsync_RemovesItemSuccessfullyAndRestoresQuantity()
        {
            var racket = new Racket
            {
                Id = 1,
                Brand = "Tecnifibre",
                BrandBg = "Технифайбър",
                Quantity = 10,
                Model = "TFight",
                ModelBg = "ТиФайт",
                ImageUrl = "racket.jpg"
            };
            var cartItem = new RacketCart { UserId = "user1", RacketId = 1, Quantity = 3, IsOrdered = false };

            dbContext.Rackets.Add(racket);
            dbContext.RacketCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            var result = await racketCartService.RemoveRacketFromCartAsync("user1", 1);

            var itemInDb = await dbContext.RacketCart.FirstOrDefaultAsync();
            var updatedRacket = await dbContext.Rackets.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(itemInDb, Is.Null);
            Assert.That(updatedRacket!.Quantity, Is.EqualTo(13));
        }

        [Test]
        public async Task RemoveRacketFromCartAsync_PrioritizesRemovingNonGiftItems()
        {
            var racket = new Racket { Id = 1, Brand = "Wilson", BrandBg = "У", Quantity = 10, Model = "M", ModelBg = "М", ImageUrl = "u.jpg" };
            var regularItem = new RacketCart { UserId = "user1", RacketId = 1, Quantity = 2, IsOrdered = false, IsGift = false };
            var giftItem = new RacketCart { UserId = "user1", RacketId = 1, Quantity = 1, IsOrdered = false, IsGift = true };

            dbContext.Rackets.Add(racket);
            dbContext.RacketCart.AddRange(regularItem, giftItem);
            await dbContext.SaveChangesAsync();

            var result = await racketCartService.RemoveRacketFromCartAsync("user1", 1);

            var remainingItems = await dbContext.RacketCart.ToListAsync();
            var updatedRacket = await dbContext.Rackets.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(remainingItems, Has.Count.EqualTo(1));
            Assert.That(remainingItems.First().IsGift, Is.True);
            Assert.That(updatedRacket!.Quantity, Is.EqualTo(12));
        }

        [Test]
        public async Task CheckOutAllRacketsAsync_ReturnsFalse_WhenCartIsEmpty()
        {
            var result = await racketCartService.CheckOutAllRacketsAsync("user1");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CheckOutAllRacketsAsync_MarksAllActiveItemsAsOrdered()
        {
            var item1 = new RacketCart { UserId = "user1", RacketId = 1, IsOrdered = false };
            var item2 = new RacketCart { UserId = "user1", RacketId = 2, IsOrdered = false };
            var item3 = new RacketCart { UserId = "user2", RacketId = 3, IsOrdered = false };

            dbContext.RacketCart.AddRange(item1, item2, item3);
            await dbContext.SaveChangesAsync();

            var result = await racketCartService.CheckOutAllRacketsAsync("user1");

            var user1Items = await dbContext.RacketCart.Where(rc => rc.UserId == "user1").ToListAsync();
            var user2Item = await dbContext.RacketCart.FirstAsync(rc => rc.UserId == "user2");

            Assert.That(result, Is.True);
            Assert.That(user1Items.All(i => i.IsOrdered), Is.True);
            Assert.That(user2Item.IsOrdered, Is.False);
        }
    }
}
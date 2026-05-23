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
    public class BallCartServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private BallCartService ballCartService;
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

            ballCartService = new BallCartService(dbContext, mockUserManager.Object, mockRankingService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllBallsInCartAsync_ReturnsEmpty_WhenUserNotFound()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

            var result = await ballCartService.GetAllBallsInCartAsync("invalid-user");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllBallsInCartAsync_CalculatesDiscountCorrectly_ForRankOne()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId, FirstName = "Jane", LastName = "Doe" };

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var ball = new Ball
            {
                Id = 1,
                Brand = "Penn",
                BrandBg = "Пен",
                Price = 10,
                Quantity = 50,
                Model = "Championship",
                ModelBg = "Шампионски",
                ImageUrl = "ball.jpg"
            };
            var cartItem = new BallCart { UserId = userId, BallId = 1, Quantity = 2, IsOrdered = false, IsGift = false };

            dbContext.Balls.Add(ball);
            dbContext.BallCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            mockRankingService.Setup(rs => rs.GetLeaderboardAsync())
                .ReturnsAsync(new List<UserRankingViewModel>
                {
                    new UserRankingViewModel { FullName = "Jane Doe", Position = 1 }
                });

            var result = await ballCartService.GetAllBallsInCartAsync(userId);
            var item = result.First();

            Assert.That(item.Price, Is.EqualTo(8m));
            Assert.That(item.TotalPrice, Is.EqualTo(16m));
        }

        [Test]
        public async Task GetAllBallsInCartAsync_SetsPriceToZero_WhenItemIsGift()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId, FirstName = "Jane", LastName = "Doe" };

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockRankingService.Setup(rs => rs.GetLeaderboardAsync()).ReturnsAsync(new List<UserRankingViewModel>());

            var ball = new Ball
            {
                Id = 1,
                Brand = "Dunlop",
                BrandBg = "Дънлоп",
                Price = 15,
                Quantity = 50,
                Model = "Fort",
                ModelBg = "Форт",
                ImageUrl = "ball.jpg"
            };
            var cartItem = new BallCart { UserId = userId, BallId = 1, Quantity = 3, IsOrdered = false, IsGift = true };

            dbContext.Balls.Add(ball);
            dbContext.BallCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            var result = await ballCartService.GetAllBallsInCartAsync(userId);
            var item = result.First();

            Assert.That(item.Price, Is.EqualTo(0m));
            Assert.That(item.TotalPrice, Is.EqualTo(0m));
        }

        [Test]
        public void AddBallToCartAsync_ThrowsException_WhenBallDoesNotExist()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await ballCartService.AddBallToCartAsync("user1", 99, 1));
        }

        [Test]
        public async Task AddBallToCartAsync_ThrowsException_WhenQuantityIsInvalid()
        {
            var ball = new Ball
            {
                Id = 1,
                Brand = "Slazenger",
                BrandBg = "Слазенгер",
                Quantity = 5,
                Model = "Wimbledon",
                ModelBg = "Уимбълдън",
                ImageUrl = "ball.jpg"
            };
            dbContext.Balls.Add(ball);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await ballCartService.AddBallToCartAsync("user1", 1, 0));

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await ballCartService.AddBallToCartAsync("user1", 1, 10));
        }

        [Test]
        public async Task AddBallToCartAsync_AddsNewItem_WhenNotInCart()
        {
            var ball = new Ball
            {
                Id = 1,
                Brand = "Wilson",
                BrandBg = "Уилсън",
                Quantity = 20,
                Model = "US Open",
                ModelBg = "ЮС Оупън",
                ImageUrl = "ball.jpg"
            };
            dbContext.Balls.Add(ball);
            await dbContext.SaveChangesAsync();

            var result = await ballCartService.AddBallToCartAsync("user1", 1, 5);

            var cartItem = await dbContext.BallCart.FirstOrDefaultAsync();
            var updatedBall = await dbContext.Balls.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(cartItem, Is.Not.Null);
            Assert.That(cartItem!.Quantity, Is.EqualTo(5));
            Assert.That(cartItem.IsGift, Is.False);
            Assert.That(cartItem.IsOrdered, Is.False);
            Assert.That(updatedBall!.Quantity, Is.EqualTo(15));
        }

        [Test]
        public async Task AddBallToCartAsync_IncreasesQuantity_WhenAlreadyInCartAndNotOrdered()
        {
            var ball = new Ball
            {
                Id = 1,
                Brand = "Head",
                BrandBg = "Хед",
                Quantity = 20,
                Model = "Tour",
                ModelBg = "Тур",
                ImageUrl = "ball.jpg"
            };
            var existingCartItem = new BallCart { UserId = "user1", BallId = 1, Quantity = 2, IsOrdered = false, IsGift = false };

            dbContext.Balls.Add(ball);
            dbContext.BallCart.Add(existingCartItem);
            await dbContext.SaveChangesAsync();

            var result = await ballCartService.AddBallToCartAsync("user1", 1, 3);

            var updatedCartItem = await dbContext.BallCart.FirstAsync();
            var updatedBall = await dbContext.Balls.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedCartItem.Quantity, Is.EqualTo(5));
            Assert.That(updatedBall!.Quantity, Is.EqualTo(17));
        }

        [Test]
        public async Task AddBallToCartAsync_ResetsItem_WhenAlreadyInCartButOrdered()
        {
            var ball = new Ball
            {
                Id = 1,
                Brand = "Babolat",
                BrandBg = "Баболат",
                Quantity = 20,
                Model = "Gold",
                ModelBg = "Голд",
                ImageUrl = "ball.jpg"
            };
            var existingCartItem = new BallCart { UserId = "user1", BallId = 1, Quantity = 5, IsOrdered = true, IsGift = false };

            dbContext.Balls.Add(ball);
            dbContext.BallCart.Add(existingCartItem);
            await dbContext.SaveChangesAsync();

            var result = await ballCartService.AddBallToCartAsync("user1", 1, 4);

            var updatedCartItem = await dbContext.BallCart.FirstAsync();
            var updatedBall = await dbContext.Balls.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedCartItem.Quantity, Is.EqualTo(4));
            Assert.That(updatedCartItem.IsOrdered, Is.False);
            Assert.That(updatedBall!.Quantity, Is.EqualTo(16));
        }

        [Test]
        public void RemoveBallFromCartAsync_ThrowsException_WhenItemNotFound()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await ballCartService.RemoveBallFromCartAsync("user1", 1));
        }

        [Test]
        public async Task RemoveBallFromCartAsync_RemovesItemSuccessfullyAndRestoresQuantity()
        {
            var ball = new Ball
            {
                Id = 1,
                Brand = "Tecnifibre",
                BrandBg = "Технифайбър",
                Quantity = 10,
                Model = "Court",
                ModelBg = "Корт",
                ImageUrl = "ball.jpg"
            };
            var cartItem = new BallCart { UserId = "user1", BallId = 1, Quantity = 3, IsOrdered = false };

            dbContext.Balls.Add(ball);
            dbContext.BallCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            var result = await ballCartService.RemoveBallFromCartAsync("user1", 1);

            var itemInDb = await dbContext.BallCart.FirstOrDefaultAsync();
            var updatedBall = await dbContext.Balls.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(itemInDb, Is.Null);
            Assert.That(updatedBall!.Quantity, Is.EqualTo(13));
        }

        [Test]
        public async Task RemoveBallFromCartAsync_PrioritizesRemovingNonGiftItems()
        {
            var ball = new Ball { Id = 1, Brand = "Wilson", BrandBg = "У", Quantity = 10, Model = "M", ModelBg = "М", ImageUrl = "u.jpg" };
            var regularItem = new BallCart { UserId = "user1", BallId = 1, Quantity = 2, IsOrdered = false, IsGift = false };
            var giftItem = new BallCart { UserId = "user1", BallId = 1, Quantity = 1, IsOrdered = false, IsGift = true };

            dbContext.Balls.Add(ball);
            dbContext.BallCart.AddRange(regularItem, giftItem);
            await dbContext.SaveChangesAsync();

            var result = await ballCartService.RemoveBallFromCartAsync("user1", 1);

            var remainingItems = await dbContext.BallCart.ToListAsync();
            var updatedBall = await dbContext.Balls.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(remainingItems, Has.Count.EqualTo(1));
            Assert.That(remainingItems.First().IsGift, Is.True);
            Assert.That(updatedBall!.Quantity, Is.EqualTo(12));
        }

        [Test]
        public async Task CheckOutAllBallsAsync_ReturnsFalse_WhenCartIsEmpty()
        {
            var result = await ballCartService.CheckOutAllBallsAsync("user1");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CheckOutAllBallsAsync_MarksAllActiveItemsAsOrdered()
        {
            var item1 = new BallCart { UserId = "user1", BallId = 1, IsOrdered = false };
            var item2 = new BallCart { UserId = "user1", BallId = 2, IsOrdered = false };
            var item3 = new BallCart { UserId = "user2", BallId = 3, IsOrdered = false };

            dbContext.BallCart.AddRange(item1, item2, item3);
            await dbContext.SaveChangesAsync();

            var result = await ballCartService.CheckOutAllBallsAsync("user1");

            var user1Items = await dbContext.BallCart.Where(bc => bc.UserId == "user1").ToListAsync();
            var user2Item = await dbContext.BallCart.FirstAsync(bc => bc.UserId == "user2");

            Assert.That(result, Is.True);
            Assert.That(user1Items.All(i => i.IsOrdered), Is.True);
            Assert.That(user2Item.IsOrdered, Is.False);
        }
    }
}
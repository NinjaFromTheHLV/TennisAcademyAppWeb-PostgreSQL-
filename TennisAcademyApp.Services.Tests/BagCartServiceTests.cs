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
    public class BagCartServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private BagCartService bagCartService;
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

            bagCartService = new BagCartService(dbContext, mockUserManager.Object, mockRankingService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllBagsInCartAsync_ReturnsEmpty_WhenUserNotFound()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

            var result = await bagCartService.GetAllBagsInCartAsync("invalid-user");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllBagsInCartAsync_CalculatesDiscountCorrectly_ForRankOne()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId, FirstName = "John", LastName = "Doe" };

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var bag = new Bag
            {
                Id = 1,
                Brand = "Wilson",
                BrandBg = "Уилсън",
                Price = 100,
                Quantity = 5,
                Model = "Pro",
                ModelBg = "Про",
                ImageUrl = "test.jpg"
            };
            var cartItem = new BagCart { UserId = userId, BagId = 1, Quantity = 2, IsOrdered = false, IsGift = false };

            dbContext.Bags.Add(bag);
            dbContext.BagCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            mockRankingService.Setup(rs => rs.GetLeaderboardAsync())
                .ReturnsAsync(new List<UserRankingViewModel>
                {
                    new UserRankingViewModel { FullName = "John Doe", Position = 1 }
                });

            var result = await bagCartService.GetAllBagsInCartAsync(userId);
            var item = result.First();

            Assert.That(item.Price, Is.EqualTo(80m));
            Assert.That(item.TotalPrice, Is.EqualTo(160m));
        }

        [Test]
        public async Task GetAllBagsInCartAsync_SetsPriceToZero_WhenItemIsGift()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId, FirstName = "John", LastName = "Doe" };

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockRankingService.Setup(rs => rs.GetLeaderboardAsync()).ReturnsAsync(new List<UserRankingViewModel>());

            var bag = new Bag
            {
                Id = 1,
                Brand = "Head",
                BrandBg = "Хед",
                Price = 100,
                Quantity = 5,
                Model = "Radical",
                ModelBg = "Радикал",
                ImageUrl = "test.jpg"
            };
            var cartItem = new BagCart { UserId = userId, BagId = 1, Quantity = 1, IsOrdered = false, IsGift = true };

            dbContext.Bags.Add(bag);
            dbContext.BagCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            var result = await bagCartService.GetAllBagsInCartAsync(userId);
            var item = result.First();

            Assert.That(item.Price, Is.EqualTo(0m));
            Assert.That(item.TotalPrice, Is.EqualTo(0m));
        }

        [Test]
        public void AddBagToCartAsync_ThrowsException_WhenBagDoesNotExist()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await bagCartService.AddBagToCartAsync("user1", 99, 1));
        }

        [Test]
        public async Task AddBagToCartAsync_ThrowsException_WhenQuantityIsInvalid()
        {
            var bag = new Bag
            {
                Id = 1,
                Brand = "Babolat",
                BrandBg = "Баболат",
                Quantity = 5,
                Model = "Aero",
                ModelBg = "Аеро",
                ImageUrl = "test.jpg"
            };
            dbContext.Bags.Add(bag);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await bagCartService.AddBagToCartAsync("user1", 1, 0));

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await bagCartService.AddBagToCartAsync("user1", 1, 10));
        }

        [Test]
        public async Task AddBagToCartAsync_AddsNewItem_WhenNotInCart()
        {
            var bag = new Bag
            {
                Id = 1,
                Brand = "Yonex",
                BrandBg = "Йонекс",
                Quantity = 10,
                Model = "Ezone",
                ModelBg = "Изоун",
                ImageUrl = "test.jpg"
            };
            dbContext.Bags.Add(bag);
            await dbContext.SaveChangesAsync();

            var result = await bagCartService.AddBagToCartAsync("user1", 1, 2);

            var cartItem = await dbContext.BagCart.FirstOrDefaultAsync();
            var updatedBag = await dbContext.Bags.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(cartItem, Is.Not.Null);
            Assert.That(cartItem!.Quantity, Is.EqualTo(2));
            Assert.That(cartItem.IsGift, Is.False);
            Assert.That(cartItem.IsOrdered, Is.False);
            Assert.That(updatedBag!.Quantity, Is.EqualTo(8));
        }

        [Test]
        public async Task AddBagToCartAsync_IncreasesQuantity_WhenAlreadyInCartAndNotOrdered()
        {
            var bag = new Bag
            {
                Id = 1,
                Brand = "Wilson",
                BrandBg = "Уилсън",
                Quantity = 10,
                Model = "Blade",
                ModelBg = "Блейд",
                ImageUrl = "test.jpg"
            };
            var existingCartItem = new BagCart { UserId = "user1", BagId = 1, Quantity = 1, IsOrdered = false, IsGift = false };

            dbContext.Bags.Add(bag);
            dbContext.BagCart.Add(existingCartItem);
            await dbContext.SaveChangesAsync();

            var result = await bagCartService.AddBagToCartAsync("user1", 1, 3);

            var updatedCartItem = await dbContext.BagCart.FirstAsync();
            var updatedBag = await dbContext.Bags.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedCartItem.Quantity, Is.EqualTo(4));
            Assert.That(updatedBag!.Quantity, Is.EqualTo(7));
        }

        [Test]
        public async Task AddBagToCartAsync_ResetsItem_WhenAlreadyInCartButOrdered()
        {
            var bag = new Bag
            {
                Id = 1,
                Brand = "Head",
                BrandBg = "Хед",
                Quantity = 10,
                Model = "Speed",
                ModelBg = "Спийд",
                ImageUrl = "test.jpg"
            };
            var existingCartItem = new BagCart { UserId = "user1", BagId = 1, Quantity = 5, IsOrdered = true, IsGift = false };

            dbContext.Bags.Add(bag);
            dbContext.BagCart.Add(existingCartItem);
            await dbContext.SaveChangesAsync();

            var result = await bagCartService.AddBagToCartAsync("user1", 1, 2);

            var updatedCartItem = await dbContext.BagCart.FirstAsync();
            var updatedBag = await dbContext.Bags.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedCartItem.Quantity, Is.EqualTo(2));
            Assert.That(updatedCartItem.IsOrdered, Is.False);
            Assert.That(updatedBag!.Quantity, Is.EqualTo(8));
        }

        [Test]
        public void RemoveBagFromCartAsync_ThrowsException_WhenItemNotFound()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await bagCartService.RemoveBagFromCartAsync("user1", 1));
        }

        [Test]
        public async Task RemoveBagFromCartAsync_RemovesItemSuccessfully()
        {
            var cartItem = new BagCart { UserId = "user1", BagId = 1, Quantity = 1, IsOrdered = false };
            dbContext.BagCart.Add(cartItem);
            await dbContext.SaveChangesAsync();

            var result = await bagCartService.RemoveBagFromCartAsync("user1", 1);
            var itemInDb = await dbContext.BagCart.FirstOrDefaultAsync();

            Assert.That(result, Is.True);
            Assert.That(itemInDb, Is.Null);
        }

        [Test]
        public async Task RemoveBagFromCartAsync_PrioritizesRemovingNonGiftItems()
        {
            var regularItem = new BagCart { UserId = "user1", BagId = 1, Quantity = 1, IsOrdered = false, IsGift = false };
            var giftItem = new BagCart { UserId = "user1", BagId = 1, Quantity = 1, IsOrdered = false, IsGift = true };

            dbContext.BagCart.AddRange(regularItem, giftItem);
            await dbContext.SaveChangesAsync();

            var result = await bagCartService.RemoveBagFromCartAsync("user1", 1);

            var remainingItems = await dbContext.BagCart.ToListAsync();

            Assert.That(result, Is.True);
            Assert.That(remainingItems, Has.Count.EqualTo(1));
            Assert.That(remainingItems.First().IsGift, Is.True);
        }

        [Test]
        public async Task CheckOutAllBagsAsync_ReturnsFalse_WhenCartIsEmpty()
        {
            var result = await bagCartService.CheckOutAllBagsAsync("user1");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CheckOutAllBagsAsync_MarksAllActiveItemsAsOrdered()
        {
            var item1 = new BagCart { UserId = "user1", BagId = 1, IsOrdered = false };
            var item2 = new BagCart { UserId = "user1", BagId = 2, IsOrdered = false };
            var item3 = new BagCart { UserId = "user2", BagId = 3, IsOrdered = false };

            dbContext.BagCart.AddRange(item1, item2, item3);
            await dbContext.SaveChangesAsync();

            var result = await bagCartService.CheckOutAllBagsAsync("user1");

            var user1Items = await dbContext.BagCart.Where(bc => bc.UserId == "user1").ToListAsync();
            var user2Item = await dbContext.BagCart.FirstAsync(bc => bc.UserId == "user2");

            Assert.That(result, Is.True);
            Assert.That(user1Items.All(i => i.IsOrdered), Is.True);
            Assert.That(user2Item.IsOrdered, Is.False);
        }
    }
}
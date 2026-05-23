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
using TennisAcademyApp.ViewModels.Racket;

namespace TennisAcademyApp.Tests
{
    [TestFixture]
    public class RacketServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private RacketService racketService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TennisAcademyDbContext>()
                .UseInMemoryDatabase(databaseName: "TennisAcademyTestDb_" + Guid.NewGuid().ToString())
                .Options;

            dbContext = new TennisAcademyDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            racketService = new RacketService(dbContext, mockUserManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllRacketsAsync_ReturnsAllRackets()
        {
            var r1 = new Racket { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "Pro", ModelBg = "Про", Price = 200, Quantity = 5, ImageUrl = "url1.jpg" };
            var r2 = new Racket { Id = 2, Brand = "Head", BrandBg = "Хед", Model = "Speed", ModelBg = "Спийд", Price = 250, Quantity = 3, ImageUrl = "url2.jpg" };

            dbContext.Rackets.AddRange(r1, r2);
            await dbContext.SaveChangesAsync();

            var result = await racketService.GetAllRacketsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void FindRacketByIdAsync_ThrowsException_WhenIdIsNull()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await racketService.FindRacketByIdAsync(null));
        }

        [Test]
        public void FindRacketByIdAsync_ThrowsException_WhenRacketNotFound()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await racketService.FindRacketByIdAsync(99));
        }

        [Test]
        public async Task FindRacketByIdAsync_ReturnsRacket_WhenFound()
        {
            var racket = new Racket { Id = 1, Brand = "Wilson", BrandBg = "У", Model = "M", ModelBg = "М", Price = 100, Quantity = 5, ImageUrl = "u.jpg" };
            dbContext.Rackets.Add(racket);
            await dbContext.SaveChangesAsync();

            var result = await racketService.FindRacketByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task AddRacketAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            var model = new RacketCreateInputModel { Brand = "W", Model = "M", Price = 10, Quantity = 1 };

            Assert.ThrowsAsync<ArgumentException>(async () => await racketService.AddRacketAsync(userId, model));
        }

        [Test]
        public async Task AddRacketAsync_AddsRacketAndReturnsTrue_WhenUserIsAdmin()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var model = new RacketCreateInputModel { Brand = "Wilson", Model = "Pro", Price = 200, Quantity = 5, ImageUrl = "url.jpg" };

            var result = await racketService.AddRacketAsync(userId, model);
            var racketInDb = await dbContext.Rackets.FirstOrDefaultAsync();

            Assert.That(result, Is.True);
            Assert.That(racketInDb, Is.Not.Null);
            Assert.That(racketInDb!.Brand, Is.EqualTo("Wilson"));
        }

        [Test]
        public void GetRacketForEdittingAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(async () => await racketService.GetRacketForEdittingAsync(userId, 1));
        }

        [Test]
        public async Task EditRacketAsync_UpdatesRacketSuccessfully()
        {
            var racket = new Racket { Id = 1, Brand = "Old", BrandBg = "О", Model = "Old", ModelBg = "О", Price = 50, Quantity = 2, ImageUrl = "old.jpg" };
            dbContext.Rackets.Add(racket);
            await dbContext.SaveChangesAsync();

            var model = new RacketEditFormModel { Id = 1, Brand = "New", Model = "New", Price = 150, Quantity = 10, ImageUrl = "new.jpg" };

            var result = await racketService.EditRacketAsync(model);
            var updatedRacket = await dbContext.Rackets.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedRacket!.Brand, Is.EqualTo("New"));
            Assert.That(updatedRacket.Price, Is.EqualTo(150));
        }

        [Test]
        public void GetRacketForDeletingAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(async () => await racketService.GetRacketForDeletingAsync(userId, 1));
        }

        [Test]
        public async Task DeleteRacketAsync_RemovesRacketSuccessfully()
        {
            var racket = new Racket { Id = 1, Brand = "Test", BrandBg = "Т", Model = "Test", ModelBg = "Т", Price = 10, Quantity = 1, ImageUrl = "t.jpg" };
            dbContext.Rackets.Add(racket);
            await dbContext.SaveChangesAsync();

            var result = await racketService.DeleteRacketAsync("admin1", 1);
            var racketInDb = await dbContext.Rackets.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(racketInDb, Is.Null);
        }
    }
}
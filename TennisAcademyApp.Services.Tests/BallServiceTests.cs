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
using TennisAcademyApp.ViewModels.Ball;

namespace TennisAcademyApp.Tests
{
    [TestFixture]
    public class BallServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private BallService ballService;
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

            ballService = new BallService(dbContext, mockUserManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllBallsAsync_ReturnsAllBalls()
        {
            var ball1 = new Ball { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "US Open", ModelBg = "ЮС Оупън", Price = 10, Quantity = 50, ImageUrl = "url1.jpg" };
            var ball2 = new Ball { Id = 2, Brand = "Head", BrandBg = "Хед", Model = "Tour", ModelBg = "Тур", Price = 12, Quantity = 30, ImageUrl = "url2.jpg" };

            dbContext.Balls.AddRange(ball1, ball2);
            await dbContext.SaveChangesAsync();

            var result = await ballService.GetAllBallsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(b => b.Id == 1), Is.True);
            Assert.That(result.Any(b => b.Id == 2), Is.True);
        }

        [Test]
        public void FindBallByIdAsync_ThrowsException_WhenIdIsNull()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await ballService.FindBallByIdAsync(null));
        }

        [Test]
        public void FindBallByIdAsync_ThrowsException_WhenBallNotFound()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await ballService.FindBallByIdAsync(99));
        }

        [Test]
        public async Task FindBallByIdAsync_ReturnsBall_WhenFound()
        {
            var ball = new Ball { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "US Open", ModelBg = "ЮС Оупън", Price = 10, Quantity = 50, ImageUrl = "url1.jpg" };
            dbContext.Balls.Add(ball);
            await dbContext.SaveChangesAsync();

            var result = await ballService.FindBallByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Brand, Is.EqualTo("Wilson"));
        }

        [Test]
        public async Task AddBallAsync_ReturnsFalse_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            var model = new BallCreateInputModel { Brand = "Wilson", Model = "US Open", Price = 10, Quantity = 50, ImageUrl = "url.jpg" };

            var result = await ballService.AddBallAsync(userId, model);

            Assert.That(result, Is.False);
            Assert.That(dbContext.Balls.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task AddBallAsync_AddsBallAndReturnsTrue_WhenUserIsAdmin()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var model = new BallCreateInputModel { Brand = "Wilson", Model = "US Open", Price = 10, Quantity = 50, ImageUrl = "url.jpg" };

            var result = await ballService.AddBallAsync(userId, model);
            var ballInDb = await dbContext.Balls.FirstOrDefaultAsync();

            Assert.That(result, Is.True);
            Assert.That(ballInDb, Is.Not.Null);
            Assert.That(ballInDb!.Brand, Is.EqualTo("Wilson"));
            Assert.That(ballInDb.Price, Is.EqualTo(10));
        }

        [Test]
        public void GetBallForEditingAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await ballService.GetBallForEditingAsync(userId, 1));
        }

        [Test]
        public async Task GetBallForEditingAsync_ReturnsModel_WhenUserIsAdminAndBallExists()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var ball = new Ball { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "US Open", ModelBg = "ЮС Оупън", Price = 10, Quantity = 50, ImageUrl = "url1.jpg" };
            dbContext.Balls.Add(ball);
            await dbContext.SaveChangesAsync();

            var result = await ballService.GetBallForEditingAsync(userId, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Brand, Is.EqualTo("Wilson"));
        }

        [Test]
        public void EditBallAsync_ThrowsException_WhenBallDoesNotExist()
        {
            var model = new BallEditFormModel { Id = 99, Brand = "Wilson", Model = "US Open", Price = 10, Quantity = 50, ImageUrl = "url.jpg" };

            Assert.ThrowsAsync<ArgumentException>(async () => await ballService.EditBallAsync(model));
        }

        [Test]
        public async Task EditBallAsync_UpdatesBallSuccessfully()
        {
            var ball = new Ball { Id = 1, Brand = "OldBrand", BrandBg = "Олд", Model = "OldModel", ModelBg = "ОлдМодел", Price = 5, Quantity = 10, ImageUrl = "old.jpg" };
            dbContext.Balls.Add(ball);
            await dbContext.SaveChangesAsync();

            var model = new BallEditFormModel { Id = 1, Brand = "NewBrand", Model = "NewModel", Price = 15, Quantity = 100, ImageUrl = "new.jpg" };

            var result = await ballService.EditBallAsync(model);
            var updatedBall = await dbContext.Balls.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedBall!.Brand, Is.EqualTo("NewBrand"));
            Assert.That(updatedBall.Price, Is.EqualTo(15));
            Assert.That(updatedBall.Quantity, Is.EqualTo(100));
        }

        [Test]
        public void GetBallForDeletingAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await ballService.GetBallForDeletingAsync(userId, 1));
        }

        [Test]
        public async Task GetBallForDeletingAsync_ReturnsModel_WhenUserIsAdminAndBallExists()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var ball = new Ball { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "US Open", ModelBg = "ЮС Оупън", Price = 10, Quantity = 50, ImageUrl = "url1.jpg" };
            dbContext.Balls.Add(ball);
            await dbContext.SaveChangesAsync();

            var result = await ballService.GetBallForDeletingAsync(userId, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void DeleteBallAsync_ThrowsException_WhenBallDoesNotExist()
        {
            var model = new BallDeleteViewModel { Id = 99 };

            Assert.ThrowsAsync<ArgumentException>(async () => await ballService.DeleteBallAsync("user1", model));
        }

        [Test]
        public async Task DeleteBallAsync_RemovesBallSuccessfully()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var ball = new Ball { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "US Open", ModelBg = "ЮС Оупън", Price = 10, Quantity = 50, ImageUrl = "url1.jpg" };
            dbContext.Balls.Add(ball);
            await dbContext.SaveChangesAsync();

            var model = new BallDeleteViewModel { Id = 1 };

            var result = await ballService.DeleteBallAsync(userId, model);
            var ballInDb = await dbContext.Balls.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(ballInDb, Is.Null);
        }
    }
}
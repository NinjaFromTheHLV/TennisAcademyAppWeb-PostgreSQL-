using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core;

namespace TennisAcademyApp.Tests
{
    [TestFixture]
    public class FavouriteCoachServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private FavouriteCoachService favouriteCoachService;
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

            favouriteCoachService = new FavouriteCoachService(dbContext, mockUserManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetFavouritesAsync_ReturnsEmpty_WhenNoFavouritesExist()
        {
            var result = await favouriteCoachService.GetFavouritesAsync("user1");
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetFavouritesAsync_ReturnsMappedViewModels_WhenFavouritesExist()
        {
            var userId = "user1";
            var coach = new Coach
            {
                CoachId = 1,
                Name = "Roger Federer",
                NameBg = "Роджър Федерер",
                Age = 40,
                Nationality = "Swiss",
                NationalityBg = "Швейцарец",
                Description = "Legend",
                DescriptionBg = "Легенда",
                ImageUrl = "roger.jpg"
            };
            var favourite = new UserFavourite { UserId = userId, CoachId = 1 };

            dbContext.Coaches.Add(coach);
            dbContext.UserFavourites.Add(favourite);
            await dbContext.SaveChangesAsync();

            var result = await favouriteCoachService.GetFavouritesAsync(userId);
            var item = result.First();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(item.CoachId, Is.EqualTo(1));
            Assert.That(item.CoachName, Is.EqualTo("Roger Federer"));
            Assert.That(item.CoachAge, Is.EqualTo(40));
            Assert.That(item.ImageUrl, Is.EqualTo("roger.jpg"));
        }

        [Test]
        public void AddFavouriteCoachAsync_ThrowsException_WhenCoachDoesNotExist()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            Assert.ThrowsAsync<ArgumentException>(async () =>
                await favouriteCoachService.AddFavouriteCoachAsync(userId, 99));
        }

        [Test]
        public async Task AddFavouriteCoachAsync_ThrowsException_WhenAlreadyInFavourites()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var coach = new Coach { CoachId = 1, Name = "Roger", NameBg = "Р", Nationality = "S", NationalityBg = "Ш", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            var favourite = new UserFavourite { UserId = userId, CoachId = 1 };

            dbContext.Coaches.Add(coach);
            dbContext.UserFavourites.Add(favourite);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async () =>
                await favouriteCoachService.AddFavouriteCoachAsync(userId, 1));
        }

        [Test]
        public async Task AddFavouriteCoachAsync_AddsFavouriteAndReturnsTrue_WhenValid()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var coach = new Coach { CoachId = 1, Name = "Roger", NameBg = "Р", Nationality = "S", NationalityBg = "Ш", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            dbContext.Coaches.Add(coach);
            await dbContext.SaveChangesAsync();

            var result = await favouriteCoachService.AddFavouriteCoachAsync(userId, 1);
            var savedFavourite = await dbContext.UserFavourites.FirstOrDefaultAsync();

            Assert.That(result, Is.True);
            Assert.That(savedFavourite, Is.Not.Null);
            Assert.That(savedFavourite!.UserId, Is.EqualTo(userId));
            Assert.That(savedFavourite.CoachId, Is.EqualTo(1));
        }

        [Test]
        public async Task RemoveFromFavouritesAsync_ReturnsFalse_WhenUserIsNull()
        {
            mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

            var coach = new Coach { CoachId = 1, Name = "Roger", NameBg = "Р", Nationality = "S", NationalityBg = "Ш", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            dbContext.Coaches.Add(coach);
            await dbContext.SaveChangesAsync();

            var result = await favouriteCoachService.RemoveFromFavouritesAsync("invalid-user", 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RemoveFromFavouritesAsync_ReturnsFalse_WhenCoachIsNull()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var result = await favouriteCoachService.RemoveFromFavouritesAsync(userId, 99);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RemoveFromFavouritesAsync_ReturnsFalse_WhenFavouriteDoesNotExist()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var coach = new Coach { CoachId = 1, Name = "Roger", NameBg = "Р", Nationality = "S", NationalityBg = "Ш", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            dbContext.Coaches.Add(coach);
            await dbContext.SaveChangesAsync();

            var result = await favouriteCoachService.RemoveFromFavouritesAsync(userId, 1);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RemoveFromFavouritesAsync_RemovesFavouriteAndReturnsTrue_WhenValid()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var coach = new Coach { CoachId = 1, Name = "Roger", NameBg = "Р", Nationality = "S", NationalityBg = "Ш", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            var favourite = new UserFavourite { UserId = userId, CoachId = 1 };

            dbContext.Coaches.Add(coach);
            dbContext.UserFavourites.Add(favourite);
            await dbContext.SaveChangesAsync();

            var result = await favouriteCoachService.RemoveFromFavouritesAsync(userId, 1);
            var remainingFavourites = await dbContext.UserFavourites.ToListAsync();

            Assert.That(result, Is.True);
            Assert.That(remainingFavourites, Is.Empty);
        }
    }
}
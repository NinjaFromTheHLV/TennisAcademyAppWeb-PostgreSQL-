using DeepL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core;
using TennisAcademyApp.ViewModels.Coach;

namespace TennisAcademyApp.Tests
{
    [TestFixture]
    public class CoachServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private CoachService coachService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<IConfiguration> mockConfiguration;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TennisAcademyDbContext>()
                .UseInMemoryDatabase(databaseName: "TennisAcademyTestDb_" + Guid.NewGuid().ToString())
                .Options;

            dbContext = new TennisAcademyDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["CoachSettings:DefaultPassword"]).Returns("Test1234!");

            var dummyTranslator = new Translator("dummy-api-key");

            coachService = new CoachService(dbContext, mockUserManager.Object, mockConfiguration.Object, dummyTranslator);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetTrainerScheduleAsync_ReturnsCorrectSchedule_ForSpecificCoach()
        {
            var coachId = 1;
            var userId = "coach1";
            var coach = new Coach { CoachId = coachId, UserId = userId, Name = "Test Coach", NameBg = "Т", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "c.jpg" };

            var playerUser = new ApplicationUser { Id = "player1", UserName = "player", Email = "player@test.com" };
            var surface = new Surface { Id = 1, Name = "Clay", NameBg = "Клей", ImageUrl = "s.jpg" };
            var trainingType = new TrainingType { Id = 1, Name = "Single", NameBg = "Единична" };

            var reservation = new Reservation
            {
                Id = 1,
                CoachId = coachId,
                Coach = coach,
                PlayerId = "player1",
                Player = playerUser,
                SurfaceId = 1,
                Surface = surface,
                TrainingTypeId = 1,
                TrainingType = trainingType,
                Date = DateTime.UtcNow.AddDays(1),
                Duration = 60,
                IsDeleted = false
            };

            dbContext.Coaches.Add(coach);
            dbContext.Users.Add(playerUser);
            dbContext.Surfaces.Add(surface);
            dbContext.Trainings.Add(trainingType);
            dbContext.Reservations.Add(reservation);
            await dbContext.SaveChangesAsync();

            var result = await coachService.GetTrainerScheduleAsync(userId);
            var schedule = result.First();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(schedule.ReservationId, Is.EqualTo(1));
            Assert.That(schedule.PlayerName, Is.EqualTo("player"));
            Assert.That(schedule.SurfaceName, Is.EqualTo("Clay"));
        }

        [Test]
        public async Task GetCoachesByPageAsync_ReturnsPaginatedCoaches()
        {
            var c1 = new Coach { CoachId = 1, Name = "Roger", NameBg = "Р", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "url1.jpg" };
            var c2 = new Coach { CoachId = 2, Name = "Rafa", NameBg = "Р", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "url2.jpg" };
            var c3 = new Coach { CoachId = 3, Name = "Novak", NameBg = "Н", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "url3.jpg" };

            dbContext.Coaches.AddRange(c1, c2, c3);
            await dbContext.SaveChangesAsync();

            var result = await coachService.GetCoachesByPageAsync(null, 1, 2);

            Assert.That(result.Coaches.Count(), Is.EqualTo(2));
            Assert.That(result.TotalPages, Is.EqualTo(2));
            Assert.That(result.PageNumber, Is.EqualTo(1));
        }

        [Test]
        public async Task GetCoachesByPageAsync_FiltersCoachesBySearchQuery()
        {
            var c1 = new Coach { CoachId = 1, Name = "Roger", NameBg = "Роджър", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "url1.jpg" };
            var c2 = new Coach { CoachId = 2, Name = "Rafa", NameBg = "Рафа", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "url2.jpg" };

            dbContext.Coaches.AddRange(c1, c2);
            await dbContext.SaveChangesAsync();

            var result = await coachService.GetCoachesByPageAsync("Rog", 1, 10);

            Assert.That(result.Coaches.Count(), Is.EqualTo(1));
            Assert.That(result.Coaches.First().CoachName, Is.EqualTo("Roger"));
        }

        [Test]
        public async Task GetGoachesForDropDownAsync_ReturnsAllCoaches()
        {
            var c1 = new Coach { CoachId = 1, Name = "Roger", NameBg = "Р", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            var c2 = new Coach { CoachId = 2, Name = "Rafa", NameBg = "Р", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };

            dbContext.Coaches.AddRange(c1, c2);
            await dbContext.SaveChangesAsync();

            var result = await coachService.GetGoachesForDropDownAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetCoachDetailsAsync_ThrowsException_WhenCoachNotFound()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.GetCoachDetailsAsync("user1", 99));
        }

        [Test]
        public async Task GetCoachDetailsAsync_ReturnsCoachDetails_WhenFound()
        {
            var coach = new Coach { CoachId = 1, Name = "Roger", NameBg = "Роджър", Age = 40, Nationality = "Swiss", NationalityBg = "Швейцарец", Description = "Desc", DescriptionBg = "Опис", ImageUrl = "img.jpg" };
            dbContext.Coaches.Add(coach);
            await dbContext.SaveChangesAsync();

            var result = await coachService.GetCoachDetailsAsync("user1", 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CoachName, Is.EqualTo("Roger"));
            Assert.That(result.IsInUserFavorites, Is.False);
        }

        [Test]
        public void AddCoachAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            var model = new AddCoachInputModel { Name = "Test Coach", Age = 30, Nationality = "US", Description = "Desc" };

            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.AddCoachAsync(userId, model));
        }

        [Test]
        public void AddCoachAsync_ThrowsException_WhenEmailAlreadyExists()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
            mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

            var model = new AddCoachInputModel { Name = "Test Coach", Age = 30, Nationality = "US", Description = "Desc" };

            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.AddCoachAsync(userId, model));
        }

        [Test]
        public void GetCoachForEdittingAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.GetCoachForEdittingAsync(userId, 1));
        }

        [Test]
        public async Task GetCoachForEdittingAsync_ReturnsModel_WhenUserIsAdminAndCoachExists()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var coach = new Coach { CoachId = 1, Name = "Roger", NameBg = "Роджър", Nationality = "Swiss", NationalityBg = "Ш", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg", Age = 40 };
            dbContext.Coaches.Add(coach);
            await dbContext.SaveChangesAsync();

            var result = await coachService.GetCoachForEdittingAsync(userId, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CoachId, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Roger"));
        }

        [Test]
        public void EdittedCoachAsync_ThrowsException_WhenCoachDoesNotExist()
        {
            var model = new CoachEditInputModel { CoachId = 99, Name = "Test" };
            mockUserManager.Setup(um => um.FindByIdAsync("user1")).ReturnsAsync(new ApplicationUser());

            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.EdittedCoachAsync("user1", model));
        }

        [Test]
        public async Task EdittedCoachAsync_UpdatesCoachSuccessfully()
        {
            var userId = "admin1";
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());

            var coach = new Coach { CoachId = 1, Name = "Old", NameBg = "Олд", Nationality = "Old", NationalityBg = "Олд", Description = "Old", DescriptionBg = "Олд", ImageUrl = "i.jpg", Age = 30 };
            dbContext.Coaches.Add(coach);
            await dbContext.SaveChangesAsync();

            var model = new CoachEditInputModel { CoachId = 1, Name = "New", Nationality = "New", Description = "New", Age = 35 };

            var result = await coachService.EdittedCoachAsync(userId, model);
            var updatedCoach = await dbContext.Coaches.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedCoach!.Name, Is.EqualTo("New"));
            Assert.That(updatedCoach.Age, Is.EqualTo(35));
        }

        [Test]
        public void GetCoachForDeletingAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.GetCoachForDeletingAsync(userId, 1));
        }

        [Test]
        public void DeletedCoachAsync_ThrowsException_WhenCoachDoesNotExist()
        {
            var model = new DeleteCoachViewModel { CoachId = 99 };
            mockUserManager.Setup(um => um.FindByIdAsync("admin1")).ReturnsAsync(new ApplicationUser());

            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.DeletedCoachAsync("admin1", model));
        }

        [Test]
        public async Task DeletedCoachAsync_RemovesCoachSuccessfully()
        {
            var coach = new Coach { CoachId = 1, Name = "Test", NameBg = "Т", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            dbContext.Coaches.Add(coach);
            await dbContext.SaveChangesAsync();

            var model = new DeleteCoachViewModel { CoachId = 1 };
            mockUserManager.Setup(um => um.FindByIdAsync("admin1")).ReturnsAsync(new ApplicationUser());

            var result = await coachService.DeletedCoachAsync("admin1", model);
            var coachInDb = await dbContext.Coaches.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(coachInDb, Is.Null);
        }

        [Test]
        public void GetCoachByIdAsync_ThrowsException_WhenIdIsNull()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.GetCoachByIdAsync(null));
        }

        [Test]
        public void GetCoachByIdAsync_ThrowsException_WhenCoachNotFound()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await coachService.GetCoachByIdAsync(99));
        }
    }
}
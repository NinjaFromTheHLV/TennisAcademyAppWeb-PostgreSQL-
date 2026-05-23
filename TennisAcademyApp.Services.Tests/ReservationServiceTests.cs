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
using TennisAcademyApp.ViewModels.Reservation;

namespace TennisAcademyApp.Tests
{
    [TestFixture]
    public class ReservationServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private ReservationService reservationService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<IDateTimeProvider> mockDateTimeProvider;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TennisAcademyDbContext>()
                .UseInMemoryDatabase(databaseName: "TennisAcademyTestDb_" + Guid.NewGuid().ToString())
                .Options;

            dbContext = new TennisAcademyDbContext(options);

            var store = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            mockDateTimeProvider = new Mock<IDateTimeProvider>();
            mockDateTimeProvider.Setup(d => d.Now).Returns(new DateTime(2026, 5, 25, 10, 0, 0));

            reservationService = new ReservationService(dbContext, mockUserManager.Object, mockDateTimeProvider.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetFilteredUserReservationsAsync_ReturnsOnlyUserReservations()
        {
            var userId = "user1";
            var reservationDate = new DateTime(2026, 5, 26, 12, 0, 0);

            var coach = new Coach { CoachId = 1, Name = "C", NameBg = "Ц", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            var type = new TrainingType { Id = 1, Name = "S", NameBg = "С" };
            dbContext.Coaches.Add(coach);
            dbContext.Trainings.Add(type);

            var res1 = new Reservation { Id = 1, PlayerId = userId, CoachId = 1, TrainingTypeId = 1, Date = reservationDate };
            var res2 = new Reservation { Id = 2, PlayerId = "other", CoachId = 1, TrainingTypeId = 1, Date = reservationDate };

            dbContext.Reservations.AddRange(res1, res2);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await reservationService.GetFilteredUserReservationsAsync(userId, null, null, null, null);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1), "Резултатът трябва да съдържа точно 1 резервация за user1");
            Assert.That(result.First().ReservationId, Is.EqualTo(1));
        }

        [Test]
        public async Task CreateReservationAsync_ReturnsTrue_WhenCoachIsAvailable()
        {
            var userId = "user1";
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser { Id = userId });

            var coach = new Coach { CoachId = 1, Name = "Test", NameBg = "Т", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            var surface = new Surface { Id = 1, Name = "S", NameBg = "С", ImageUrl = "s.jpg" };
            var type = new TrainingType { Id = 1, Name = "T", NameBg = "Т" };

            dbContext.Coaches.Add(coach);
            dbContext.Surfaces.Add(surface);
            dbContext.Trainings.Add(type);
            await dbContext.SaveChangesAsync();

            var model = new ReservationCreateInputModel
            {
                CoachId = 1,
                SurfaceId = 1,
                TrainingTypeId = 1,
                Date = new DateTime(2026, 5, 26, 10, 0, 0),
                Duration = 60
            };

            var result = await reservationService.CreateReservationAsync(userId, model);

            Assert.That(result, Is.True);
            Assert.That(dbContext.Reservations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void DateValidationAsync_ThrowsException_WhenDateIsPast()
        {
            var model = new ReservationCreateInputModel { Date = new DateTime(2026, 5, 24) };
            Assert.ThrowsAsync<ArgumentException>(async () => await reservationService.DateValidationAsync(model));
        }

        [Test]
        public void DateValidationAsync_ThrowsException_WhenDateIsSunday()
        {
            var model = new ReservationCreateInputModel { Date = new DateTime(2026, 5, 31, 10, 0, 0) };
            Assert.ThrowsAsync<ArgumentException>(async () => await reservationService.DateValidationAsync(model));
        }

        [Test]
        public async Task IsCoachAvailableAtTheTimeAsync_ThrowsException_WhenOverlapExists()
        {
            var coach = new Coach { CoachId = 1, Name = "T", NameBg = "Т", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            dbContext.Coaches.Add(coach);

            var existing = new Reservation
            {
                CoachId = 1,
                PlayerId = "p",
                Date = new DateTime(2026, 5, 26, 10, 0, 0),
                Duration = 60
            };
            dbContext.Reservations.Add(existing);
            await dbContext.SaveChangesAsync();

            var model = new ReservationCreateInputModel
            {
                CoachId = 1,
                Date = new DateTime(2026, 5, 26, 10, 30, 0),
                Duration = 60
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await reservationService.IsCoachAvailableAtTheTimeAsync(model));
        }

        [Test]
        public async Task AutoReservationDeleteAsync_MarksExpiredReservationsAsCompleted()
        {
            var expired = new Reservation { Id = 1, PlayerId = "p", Date = new DateTime(2026, 5, 24), IsCompleted = false };
            dbContext.Reservations.Add(expired);
            await dbContext.SaveChangesAsync();

            var result = await reservationService.AutoReservationDeleteAsync();

            var updated = await dbContext.Reservations.FindAsync(1);
            Assert.That(result, Is.True);
            Assert.That(updated!.IsCompleted, Is.True);
        }

        [Test]
        public async Task GetUserReservationHistoryAsync_ReturnsPastReservations()
        {
            var userId = "user1";

            var coach = new Coach { CoachId = 1, Name = "C", NameBg = "Ц", Nationality = "N", NationalityBg = "Н", Description = "D", DescriptionBg = "Д", ImageUrl = "i.jpg" };
            var surface = new Surface { Id = 1, Name = "S", NameBg = "С", ImageUrl = "s.jpg" };
            var trainingType = new TrainingType { Id = 1, Name = "T", NameBg = "Т" };

            dbContext.Coaches.Add(coach);
            dbContext.Surfaces.Add(surface);
            dbContext.Trainings.Add(trainingType);

            var past = new Reservation
            {
                Id = 1,
                PlayerId = userId,
                CoachId = 1,
                SurfaceId = 1,
                TrainingTypeId = 1,
                IsCompleted = true,
                Date = new DateTime(2026, 1, 1)
            };

            dbContext.Reservations.Add(past);
            await dbContext.SaveChangesAsync();

            var result = await reservationService.GetUserReservationHistoryAsync(userId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1), "Резултатът трябва да върне 1 резервация.");
        }
    }
}
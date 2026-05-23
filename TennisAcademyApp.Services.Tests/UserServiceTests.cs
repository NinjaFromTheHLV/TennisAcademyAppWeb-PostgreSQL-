using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core;
using TennisAcademyApp.ViewModels.Admin.UserManagement;

namespace TennisAcademyApp.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private UserService userService;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private Mock<RoleManager<IdentityRole>> mockRoleManager;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TennisAcademyDbContext>()
                .UseInMemoryDatabase(databaseName: "TennisAcademyTestDb_" + Guid.NewGuid().ToString())
                .Options;

            dbContext = new TennisAcademyDbContext(options);

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            mockRoleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            userService = new UserService(mockUserManager.Object, mockRoleManager.Object, dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetUserManagementDataAsync_ExcludesCurrentUser()
        {
            var user1 = new ApplicationUser { Id = "admin", Email = "admin@test.com" };
            var user2 = new ApplicationUser { Id = "user", Email = "user@test.com" };

            var usersList = new List<ApplicationUser> { user1, user2 };

            var usersQueryable = usersList.AsQueryable().BuildMockDbSet();

            mockUserManager.Setup(um => um.Users).Returns(usersList.AsQueryable().BuildMock());

            mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
                           .ReturnsAsync(new List<string>());

            var roles = new List<IdentityRole> { new IdentityRole { Name = "Admin" } }.AsQueryable();
            mockRoleManager.Setup(rm => rm.Roles).Returns(roles.BuildMock());

            var result = await userService.GetUserManagementDataAsync("admin");

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo("user"));
        }

        [Test]
        public async Task AssignUserToRoleAsync_ReturnsFalse_WhenUserOrRoleMissing()
        {
            mockUserManager.Setup(um => um.FindByIdAsync("invalid")).ReturnsAsync((ApplicationUser)null!);

            var result = await userService.AssignUserToRoleAsync("invalid", "Admin");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AssignUserToRoleAsync_AssignsRoleAndCreatesCoach_WhenRoleIsCoach()
        {
            // Arrange
            var userId = "user1";
            var user = new ApplicationUser { Id = userId, FirstName = "John", LastName = "Doe" };
            string role = "Coach"; 

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockRoleManager.Setup(rm => rm.RoleExistsAsync(role)).ReturnsAsync(true);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, role)).ReturnsAsync(false);
            mockUserManager.Setup(um => um.AddToRoleAsync(user, role)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await userService.AssignUserToRoleAsync(userId, role);
            var coachInDb = await dbContext.Coaches.FirstOrDefaultAsync(c => c.UserId == userId);

            Assert.That(result, Is.True);
            Assert.That(coachInDb, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(coachInDb!.Name, Is.EqualTo("John Doe"));
                Assert.That(coachInDb.ImageUrl, Is.Not.Null);
                Assert.That(coachInDb.NameBg, Is.Not.Null);
                Assert.That(coachInDb.Nationality, Is.EqualTo("Unknown"));
            });
        }

        [Test]
        public async Task RemoveUserFromRoleAsync_RemovesRoleSuccessfully()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockRoleManager.Setup(rm => rm.RoleExistsAsync("Admin")).ReturnsAsync(true);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);
            mockUserManager.Setup(um => um.RemoveFromRoleAsync(user, "Admin")).ReturnsAsync(IdentityResult.Success);

            var result = await userService.RemoveUserFromRoleAsync(userId, "Admin");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task RemoveUserAsync_DeletesUserAndRelatedData()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };

            var coach = new Coach
            {
                CoachId = 1,
                Name = "Test",
                NameBg = "Т",
                Nationality = "N",
                NationalityBg = "Н",
                Description = "D",
                DescriptionBg = "Д",
                ImageUrl = "t.jpg"
            };
            dbContext.Coaches.Add(coach);

            dbContext.Reservations.Add(new Reservation { PlayerId = userId });
            dbContext.UserFavourites.Add(new UserFavourite { UserId = userId, CoachId = 1 });

            await dbContext.SaveChangesAsync();

            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await userService.RemoveUserAsync(userId);

            Assert.That(result, Is.True);
            Assert.That(dbContext.Reservations.Count(), Is.EqualTo(0));
            Assert.That(dbContext.UserFavourites.Count(), Is.EqualTo(0));
        }
    }
}
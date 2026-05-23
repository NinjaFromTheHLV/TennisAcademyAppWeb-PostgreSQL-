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
using TennisAcademyApp.ViewModels.Bag;

namespace TennisAcademyApp.Tests
{
    [TestFixture]
    public class BagServiceTests
    {
        private TennisAcademyDbContext dbContext;
        private BagService bagService;
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

            bagService = new BagService(dbContext, mockUserManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        [Test]
        public async Task GetAllBagsAsync_ReturnsAllBags()
        {
            var bag1 = new Bag { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "Pro", ModelBg = "Про", Price = 100, Quantity = 5, ImageUrl = "url1.jpg" };
            var bag2 = new Bag { Id = 2, Brand = "Head", BrandBg = "Хед", Model = "Speed", ModelBg = "Спийд", Price = 120, Quantity = 3, ImageUrl = "url2.jpg" };

            dbContext.Bags.AddRange(bag1, bag2);
            await dbContext.SaveChangesAsync();

            var result = await bagService.GetAllBagsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(b => b.Id == 1), Is.True);
            Assert.That(result.Any(b => b.Id == 2), Is.True);
        }

        [Test]
        public void FindBagByIdAsync_ThrowsException_WhenIdIsNull()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await bagService.FindBagByIdAsync(null));
        }

        [Test]
        public void FindBagByIdAsync_ThrowsException_WhenBagNotFound()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await bagService.FindBagByIdAsync(99));
        }

        [Test]
        public async Task FindBagByIdAsync_ReturnsBag_WhenFound()
        {
            var bag = new Bag { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "Pro", ModelBg = "Про", Price = 100, Quantity = 5, ImageUrl = "url1.jpg" };
            dbContext.Bags.Add(bag);
            await dbContext.SaveChangesAsync();

            var result = await bagService.FindBagByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Brand, Is.EqualTo("Wilson"));
        }

        [Test]
        public async Task AddBagAsync_ReturnsFalse_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            var model = new BagCreateInputModel { Brand = "Wilson", Model = "Pro", Price = 100, Quantity = 5, ImageUrl = "url.jpg" };

            var result = await bagService.AddBagAsync(userId, model);

            Assert.That(result, Is.False);
            Assert.That(dbContext.Bags.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task AddBagAsync_AddsBagAndReturnsTrue_WhenUserIsAdmin()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var model = new BagCreateInputModel { Brand = "Wilson", Model = "Pro", Price = 100, Quantity = 5, ImageUrl = "url.jpg" };

            var result = await bagService.AddBagAsync(userId, model);
            var bagInDb = await dbContext.Bags.FirstOrDefaultAsync();

            Assert.That(result, Is.True);
            Assert.That(bagInDb, Is.Not.Null);
            Assert.That(bagInDb!.Brand, Is.EqualTo("Wilson"));
            Assert.That(bagInDb.Price, Is.EqualTo(100));
        }

        [Test]
        public void GetBagForEditingAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(async () => await bagService.GetBagForEditingAsync(userId, 1));
        }

        [Test]
        public async Task GetBagForEditingAsync_ReturnsModel_WhenUserIsAdminAndBagExists()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var bag = new Bag { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "Pro", ModelBg = "Про", Price = 100, Quantity = 5, ImageUrl = "url1.jpg" };
            dbContext.Bags.Add(bag);
            await dbContext.SaveChangesAsync();

            var result = await bagService.GetBagForEditingAsync(userId, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Brand, Is.EqualTo("Wilson"));
        }

        [Test]
        public void EditBagAsync_ThrowsException_WhenBagDoesNotExist()
        {
            var model = new BagEditFormModel { Id = 99, Brand = "Wilson", Model = "Pro", Price = 100, Quantity = 5, ImageUrl = "url.jpg" };

            Assert.ThrowsAsync<ArgumentException>(async () => await bagService.EditBagAsync(model));
        }

        [Test]
        public async Task EditBagAsync_UpdatesBagSuccessfully()
        {
            var bag = new Bag { Id = 1, Brand = "OldBrand", BrandBg = "Олд", Model = "OldModel", ModelBg = "ОлдМодел", Price = 50, Quantity = 2, ImageUrl = "old.jpg" };
            dbContext.Bags.Add(bag);
            await dbContext.SaveChangesAsync();

            var model = new BagEditFormModel { Id = 1, Brand = "NewBrand", Model = "NewModel", Price = 150, Quantity = 10, ImageUrl = "new.jpg" };

            var result = await bagService.EditBagAsync(model);
            var updatedBag = await dbContext.Bags.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(updatedBag!.Brand, Is.EqualTo("NewBrand"));
            Assert.That(updatedBag.Price, Is.EqualTo(150));
            Assert.That(updatedBag.Quantity, Is.EqualTo(10));
        }

        [Test]
        public void GetBagForDeletingAsync_ThrowsException_WhenUserIsNotAdmin()
        {
            var userId = "user1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(async () => await bagService.GetBagForDeletingAsync(userId, 1));
        }

        [Test]
        public async Task GetBagForDeletingAsync_ReturnsModel_WhenUserIsAdminAndBagExists()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            mockUserManager.Setup(um => um.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var bag = new Bag { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "Pro", ModelBg = "Про", Price = 100, Quantity = 5, ImageUrl = "url1.jpg" };
            dbContext.Bags.Add(bag);
            await dbContext.SaveChangesAsync();

            var result = await bagService.GetBagForDeletingAsync(userId, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void DeleteBagAsync_ThrowsException_WhenBagDoesNotExist()
        {
            var model = new BagDeleteViewModel { Id = 99 };

            Assert.ThrowsAsync<ArgumentException>(async () => await bagService.DeleteBagAsync("user1", model));
        }

        [Test]
        public async Task DeleteBagAsync_RemovesBagSuccessfully()
        {
            var userId = "admin1";
            var user = new ApplicationUser { Id = userId };
            mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

            var bag = new Bag { Id = 1, Brand = "Wilson", BrandBg = "Уилсън", Model = "Pro", ModelBg = "Про", Price = 100, Quantity = 5, ImageUrl = "url1.jpg" };
            dbContext.Bags.Add(bag);
            await dbContext.SaveChangesAsync();

            var model = new BagDeleteViewModel { Id = 1 };

            var result = await bagService.DeleteBagAsync(userId, model);
            var bagInDb = await dbContext.Bags.FindAsync(1);

            Assert.That(result, Is.True);
            Assert.That(bagInDb, Is.Null);
        }
    }
}
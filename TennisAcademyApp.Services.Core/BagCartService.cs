using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Cart;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.BagCart;

namespace TennisAcademyApp.Services.Core
{
    public class BagCartService : IBagCartService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRankingService rankingService;

        public BagCartService(
            TennisAcademyDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IRankingService rankingService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.rankingService = rankingService;
        }

        public async Task<IEnumerable<BagCartIndexViewModel>> GetAllBagsInCartAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Enumerable.Empty<BagCartIndexViewModel>();
            }

            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            var leaderboard = await rankingService.GetLeaderboardAsync();
            var userRanking = leaderboard.FirstOrDefault(u => u.FullName == $"{user.FirstName} {user.LastName}");

            decimal discountMultiplier = 1.0m;
            if (userRanking != null)
            {
                discountMultiplier = userRanking.Position switch
                {
                    1 => 0.80m,
                    2 => 0.85m,
                    3 => 0.90m,
                    _ => 1.00m
                };
            }

            var bagsInCart = await dbContext.BagCart
                .Include(bc => bc.Bag)
                .Where(bc => bc.UserId == userId && !bc.IsOrdered)
                .Select(bc => new BagCartIndexViewModel
                {
                    Id = bc.BagId,
                    Brand = isBg ? bc.Bag.BrandBg : bc.Bag.Brand,
                    Model = isBg ? bc.Bag.ModelBg : bc.Bag.Model,
                    Price = bc.IsGift ? 0m : (bc.Bag.Price * discountMultiplier),
                    Quantity = bc.Quantity,
                    TotalPrice = bc.IsGift ? 0m : (bc.Quantity * (bc.Bag.Price * discountMultiplier)),
                    ImageUrl = bc.Bag.ImageUrl,
                    IsGift = bc.IsGift 
                })
                .ToListAsync();

            return bagsInCart;
        }

        public async Task<bool> AddBagToCartAsync(string userId, int id, int quantity)
        {
            bool result = false;
            var user = await userManager.FindByIdAsync(userId);
            var bag = await dbContext.Bags.FindAsync(id);

            if (bag == null || quantity <= 0 || quantity > bag.Quantity)
            {
                throw new InvalidOperationException(InvalidQuantityErrorMessage);
            }

            var existingItem = await dbContext.BagCart
                .FirstOrDefaultAsync(bc => bc.UserId == userId && bc.BagId == id && !bc.IsGift);

            if (existingItem != null)
            {
                if (!existingItem.IsOrdered)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    existingItem.Quantity = quantity;
                    existingItem.IsOrdered = false;
                }

                bag.Quantity -= quantity;
                result = true;
            }
            else
            {
                var cartItem = new BagCart
                {
                    UserId = userId,
                    BagId = id,
                    Quantity = quantity,
                    IsOrdered = false,
                    IsGift = false
                };
                bag.Quantity -= quantity;

                await dbContext.BagCart.AddAsync(cartItem);
                result = true;
            }

            await dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<bool> RemoveBagFromCartAsync(string userId, int bagId)
        {
            bool result = false;
            var cartItem = await dbContext.BagCart
                .Where(bc => bc.BagId == bagId && bc.UserId == userId && !bc.IsOrdered)
                .OrderBy(bc => bc.IsGift)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                throw new InvalidOperationException(BagNotFoundInCartErrorMessage);
            }

            dbContext.BagCart.Remove(cartItem);
            await dbContext.SaveChangesAsync();

            result = true;
            return result;
        }

        public async Task<bool> CheckOutAllBagsAsync(string userId)
        {
            bool result = false;

            var cartItems = await dbContext.BagCart
                .Where(bc => bc.UserId == userId && !bc.IsOrdered)
                .ToListAsync();

            if (cartItems.Any())
            {
                foreach (var item in cartItems)
                {
                    item.IsOrdered = true;
                }

                await dbContext.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
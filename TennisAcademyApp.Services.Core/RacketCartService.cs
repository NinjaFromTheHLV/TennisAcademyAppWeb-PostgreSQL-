using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Cart;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.RacketCart;

namespace TennisAcademyApp.Services.Core
{
    public class RacketCartService : IRacketCartService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRankingService rankingService;

        public RacketCartService(
            TennisAcademyDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IRankingService rankingService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.rankingService = rankingService;
        }

        public async Task<IEnumerable<RacketCartIndexViewModel>> GetAllRacketsInCartAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Enumerable.Empty<RacketCartIndexViewModel>();

            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            var leaderboard = await rankingService.GetLeaderboardAsync();
            var userRanking = leaderboard.FirstOrDefault(u => u.FullName == $"{user.FirstName} {user.LastName}");

            decimal discountMultiplier = userRanking?.Position switch
            {
                1 => 0.80m,
                2 => 0.85m,
                3 => 0.90m,
                _ => 1.00m
            };

            return await dbContext.RacketCart
                .Include(rc => rc.Racket)
                .Where(rc => rc.UserId == userId && !rc.IsOrdered)
                .Select(rc => new RacketCartIndexViewModel
                {
                    Id = rc.RacketId,
                    Brand = isBg ? rc.Racket.BrandBg : rc.Racket.Brand,
                    Model = isBg ? rc.Racket.ModelBg : rc.Racket.Model,
                    Price = rc.IsGift ? 0m : (rc.Racket.Price * discountMultiplier),
                    Quantity = rc.Quantity,
                    TotalPrice = rc.IsGift ? 0m : (rc.Quantity * (rc.Racket.Price * discountMultiplier)),
                    ImageUrl = rc.Racket.ImageUrl,
                    IsGift = rc.IsGift
                })
                .ToListAsync();
        }

        public async Task<bool> AddRacketToCartAsync(string userId, int id, int quantity)
        {
            var racket = await dbContext.Rackets.FindAsync(id);
            if (racket == null || quantity <= 0 || quantity > racket.Quantity)
                throw new InvalidOperationException(InvalidQuantityErrorMessage);

            var existingItem = await dbContext.RacketCart
                .FirstOrDefaultAsync(rc => rc.UserId == userId && rc.RacketId == id && !rc.IsGift);

            if (existingItem != null)
            {
                existingItem.Quantity += (!existingItem.IsOrdered) ? quantity : 0;
                existingItem.IsOrdered = false;
            }
            else
            {
                await dbContext.RacketCart.AddAsync(new RacketCart
                {
                    UserId = userId,
                    RacketId = id,
                    Quantity = quantity,
                    IsOrdered = false,
                    IsGift = false
                });
            }

            racket.Quantity -= quantity;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRacketFromCartAsync(string userId, int id)
        {
            var cartItem = await dbContext.RacketCart
                .Where(rc => rc.RacketId == id && rc.UserId == userId && !rc.IsOrdered)
                .OrderBy(rc => rc.IsGift)
                .FirstOrDefaultAsync();

            if (cartItem == null) throw new InvalidOperationException(RacketNotFoundInCartErrorMessage);

            var racket = await dbContext.Rackets.FindAsync(id);
            if (racket != null) racket.Quantity += cartItem.Quantity;

            dbContext.RacketCart.Remove(cartItem);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckOutAllRacketsAsync(string userId)
        {
            var cartItems = await dbContext.RacketCart
                .Where(rc => rc.UserId == userId && !rc.IsOrdered)
                .ToListAsync();

            if (!cartItems.Any()) return false;

            foreach (var item in cartItems)
            {
                item.IsOrdered = true;
            }

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
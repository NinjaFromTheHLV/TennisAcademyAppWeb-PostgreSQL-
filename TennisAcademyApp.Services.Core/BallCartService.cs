using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Cart;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.BallCart;

namespace TennisAcademyApp.Services.Core
{
    public class BallCartService : IBallCartService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRankingService rankingService;

        public BallCartService(
            TennisAcademyDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IRankingService rankingService)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.rankingService = rankingService;
        }

        public async Task<IEnumerable<BallCartIndexViewModel>> GetAllBallsInCartAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Enumerable.Empty<BallCartIndexViewModel>();
            }

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

            return await dbContext.BallCart
                .Include(bc => bc.Ball)
                .Where(bc => bc.UserId == userId && !bc.IsOrdered)
                .Select(bc => new BallCartIndexViewModel
                {
                    Id = bc.BallId,
                    Brand = isBg ? bc.Ball.BrandBg : bc.Ball.Brand,
                    Model = isBg ? bc.Ball.ModelBg : bc.Ball.Model,
                    // Ако е подарък, цената и общата сума са 0
                    Price = bc.IsGift ? 0m : (bc.Ball.Price * discountMultiplier),
                    Quantity = bc.Quantity,
                    TotalPrice = bc.IsGift ? 0m : (bc.Quantity * (bc.Ball.Price * discountMultiplier)),
                    ImageUrl = bc.Ball.ImageUrl,
                    IsGift = bc.IsGift
                })
                .ToListAsync();
        }

        public async Task<bool> AddBallToCartAsync(string userId, int ballId, int quantity)
        {
            var ball = await dbContext.Balls.FindAsync(ballId);
            if (ball == null || quantity <= 0 || quantity > ball.Quantity)
            {
                throw new InvalidOperationException(InvalidQuantityErrorMessage);
            }

            // Търсим само артикули, които НЕ са подаръци, за да ги групираме правилно
            var existingItem = await dbContext.BallCart
                .FirstOrDefaultAsync(bc => bc.UserId == userId && bc.BallId == ballId && !bc.IsGift);

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
            }
            else
            {
                var cartItem = new BallCart
                {
                    UserId = userId,
                    BallId = ballId,
                    Quantity = quantity,
                    IsOrdered = false,
                    IsGift = false // Явно указваме, че е купен артикул
                };
                await dbContext.BallCart.AddAsync(cartItem);
            }

            ball.Quantity -= quantity;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveBallFromCartAsync(string userId, int ballId)
        {
            var cartItem = await dbContext.BallCart
                .Where(bc => bc.BallId == ballId && bc.UserId == userId && !bc.IsOrdered)
                .OrderBy(bc => bc.IsGift)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                throw new InvalidOperationException(BallNotFoundInCartErrorMessage);
            }

            var ball = await dbContext.Balls.FindAsync(ballId);
            if (ball != null)
            {
                ball.Quantity += cartItem.Quantity;
            }

            dbContext.BallCart.Remove(cartItem);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckOutAllBallsAsync(string userId)
        {
            var cartItems = await dbContext.BallCart
                .Where(bc => bc.UserId == userId && !bc.IsOrdered)
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
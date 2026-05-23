using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.ViewModels.Wheel;
using TennisAcademyApp.Services.Contracts;

namespace TennisAcademyApp.Services
{
    public class WheelService : IWheelService
    {
        private readonly TennisAcademyDbContext _context;

        public WheelService(TennisAcademyDbContext context)
        {
            _context = context;
        }

        public async Task<(string RacketName, string BallName, string BagName)> GetWheelItemsAsync()
        {
            var racket = await _context.Rackets.FirstOrDefaultAsync();
            var ball = await _context.Balls.FirstOrDefaultAsync();
            var bag = await _context.Bags.FirstOrDefaultAsync();

            return (
                racket?.Model ?? "Racket",
                ball?.Brand ?? "Balls",
                bag?.Brand ?? "Bag"
            );
        }

        public async Task<(bool CanSpin, DateTime? NextSpinDate)> GetUserEligibilityAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return (false, null);

            bool canSpin = !user.LastWheelSpinDate.HasValue ||
                           (DateTime.UtcNow - user.LastWheelSpinDate.Value).TotalDays >= 7;

            DateTime? nextSpin = canSpin ? null : user.LastWheelSpinDate.Value.AddDays(7);

            return (canSpin, nextSpin);
        }

        public async Task<WheelSpinResultViewModel> SpinWheelAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new WheelSpinResultViewModel { Success = false, ErrorMessage = "User not found." };

            bool canSpin = !user.LastWheelSpinDate.HasValue ||
                           (DateTime.UtcNow - user.LastWheelSpinDate.Value).TotalDays >= 7;

            if (!canSpin)
            {
                return new WheelSpinResultViewModel { Success = false, ErrorMessage = "Вече сте въртели колелото тази седмица!" };
            }

            int winningIndex = Random.Shared.Next(0, 6);
            string prizeName = "ОПИТАЙ ПАК";

            if (winningIndex == 0)
            {
                var item = await _context.Rackets.Select(r => new { r.Id, r.Model }).FirstOrDefaultAsync();
                if (item != null)
                {
                    prizeName = item.Model;
                    var cartItem = await _context.RacketCart.FirstOrDefaultAsync(c => c.UserId == userId && c.RacketId == item.Id && c.IsGift == true);
                    if (cartItem != null) cartItem.Quantity++;
                    else _context.RacketCart.Add(new RacketCart { UserId = userId, RacketId = item.Id, Quantity = 1, IsGift = true });
                }
            }
            else if (winningIndex == 2)
            {
                var item = await _context.Balls.Select(b => new { b.Id, b.Brand }).FirstOrDefaultAsync();
                if (item != null)
                {
                    prizeName = item.Brand;
                    var cartItem = await _context.BallCart.FirstOrDefaultAsync(c => c.UserId == userId && c.BallId == item.Id && c.IsGift == true);
                    if (cartItem != null) cartItem.Quantity++;
                    else _context.BallCart.Add(new BallCart { UserId = userId, BallId = item.Id, Quantity = 1, IsGift = true });
                }
            }
            else if (winningIndex == 4)
            {
                var item = await _context.Bags.Select(b => new { b.Id, b.Brand }).FirstOrDefaultAsync();
                if (item != null)
                {
                    prizeName = item.Brand;
                    var cartItem = await _context.BagCart.FirstOrDefaultAsync(c => c.UserId == userId && c.BagId == item.Id && c.IsGift == true);
                    if (cartItem != null) cartItem.Quantity++;
                    else _context.BagCart.Add(new BagCart { UserId = userId, BagId = item.Id, Quantity = 1, IsGift = true });
                }
            }

            user.LastWheelSpinDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new WheelSpinResultViewModel
            {
                Success = true,
                WinningIndex = winningIndex,
                Prize = prizeName
            };
        }
    }
}
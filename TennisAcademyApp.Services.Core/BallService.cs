using GTranslate.Translators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.GCommon.TextUtility;
using TennisAcademyApp.Services.Core.Contracts;
using TennisAcademyApp.ViewModels.Ball;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Ball;

namespace TennisAcademyApp.Services.Core
{
    public class BallService : IBallService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public BallService(TennisAcademyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<BallIndexViewModel>> GetAllBallsAsync()
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            var balls = await dbContext.Balls
                .AsNoTracking()
                .Include(b => b.RacketCarts)
                .Select(b => new BallIndexViewModel
                {
                    Id = b.Id,
                    Brand = isBg ? b.BrandBg : b.Brand,
                    Model = isBg ? b.ModelBg : b.Model,
                    Price = b.Price,
                    Quantity = b.Quantity,
                    ImageUrl = b.ImageUrl,
                })
                .ToListAsync();

            return balls;
        }

        public async Task<Ball> FindBallByIdAsync(int? id)
        {
            if (id.HasValue)
            {
                // ОПРАВЕНО: Махнат AsNoTracking(), за да се следи обекта при Edit операции
                var ball = await dbContext.Balls
                    .FirstOrDefaultAsync(b => b.Id == id.Value);

                if (ball == null)
                {
                    throw new ArgumentException(BallNotFoundErrorMessage);
                }

                return ball;
            }
            else
            {
                throw new ArgumentException(BallCannotBeNullErrorMessage);
            }
        }

        public async Task<bool> AddBallAsync(string userId, BallCreateInputModel model)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null || !await userManager.IsInRoleAsync(user, "Admin"))
                return false;

            // Използваме TextUtility за стабилен резултат без API заявки
            string brandBg = TextUtility.CapitalizeNames(TextUtility.TransliterateToBg(model.Brand));
            string modelBg = TextUtility.CapitalizeNames(TextUtility.TransliterateToBg(model.Model));

            var ball = new Ball
            {
                Brand = model.Brand,
                BrandBg = brandBg,
                Model = model.Model,
                ModelBg = modelBg,
                Price = model.Price,
                Quantity = model.Quantity,
                ImageUrl = model.ImageUrl ?? "~/pictures/DefaultBallImage.webp",
            };

            await dbContext.Balls.AddAsync(ball);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<BallEditFormModel> GetBallForEditingAsync(string userId, int? id)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool IsAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (!IsAdmin)
            {
                throw new UnauthorizedAccessException("You do not have permission to edit a ball.");
            }

            var ball = await FindBallByIdAsync(id);

            var model = new BallEditFormModel
            {
                Id = ball.Id,
                Brand = ball.Brand,
                Model = ball.Model,
                Price = ball.Price,
                Quantity = ball.Quantity,
                ImageUrl = ball.ImageUrl
            };

            return model;
        }

        public async Task<bool> EditBallAsync(BallEditFormModel model)
        {
            var ball = await dbContext.Balls.FirstOrDefaultAsync(b => b.Id == model.Id);
            if (ball == null)
            {
                throw new ArgumentException(BallNotFoundErrorMessage);
            }

            // Пак използваме TextUtility за консистентност
            string brandBg = TextUtility.CapitalizeNames(TextUtility.TransliterateToBg(model.Brand));
            string modelBg = TextUtility.CapitalizeNames(TextUtility.TransliterateToBg(model.Model));

            ball.Brand = model.Brand;
            ball.BrandBg = brandBg;
            ball.Model = model.Model;
            ball.ModelBg = modelBg;
            ball.Price = model.Price;
            ball.Quantity = model.Quantity;
            ball.ImageUrl = model.ImageUrl;

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BallDeleteViewModel> GetBallForDeletingAsync(string userId, int? id)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool IsAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (user == null || !IsAdmin)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete a ball.");
            }

            var ball = await FindBallByIdAsync(id);

            var currentCulture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture.Equals("bg", StringComparison.OrdinalIgnoreCase);

            var model = new BallDeleteViewModel
            {
                Id = ball.Id,
                Brand = isBg ? ball.BrandBg : ball.Brand,
                Model = isBg ? ball.ModelBg : ball.Model,
                ImageUrl = ball.ImageUrl
            };

            return model;
        }

        public async Task<bool> DeleteBallAsync(string userId, BallDeleteViewModel model)
        {
            var user = await userManager.FindByIdAsync(userId);

            var ball = await dbContext.Balls.FindAsync(model.Id);

            if (ball == null)
            {
                throw new ArgumentException(BallNotFoundErrorMessage);
            }

            dbContext.Balls.Remove(ball);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
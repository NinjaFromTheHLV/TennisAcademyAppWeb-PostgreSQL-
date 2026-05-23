using DeepL;
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
using TennisAcademyApp.ViewModels.Bag;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.Bag;

namespace TennisAcademyApp.Services.Core
{
    public class BagService : IBagService
    {
        private readonly TennisAcademyDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public BagService(TennisAcademyDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<BagIndexViewModel>> GetAllBagsAsync()
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture == "bg";

            var bags = await dbContext.Bags
                .AsNoTracking()
                .Include(b => b.BagCarts)
                .Select(b => new BagIndexViewModel
                {
                    Id = b.Id,
                    Brand = isBg ? b.BrandBg : b.Brand,
                    Model = isBg ? b.ModelBg : b.Model,
                    Price = b.Price,
                    Quantity = b.Quantity,
                    ImageUrl = b.ImageUrl,
                })
                .ToListAsync();

            return bags;
        }

        public async Task<Bag> FindBagByIdAsync(int? id)
        {
            if (id.HasValue)
            {
                var bag = await dbContext.Bags
                    .FirstOrDefaultAsync(b => b.Id == id.Value);

                if (bag == null)
                {
                    throw new ArgumentException(BagNotFoundErrorMessage);
                }

                return bag;
            }
            else
            {
                throw new ArgumentException(BagCannotBeNullErrorMessage);
            }
        }

        public async Task<bool> AddBagAsync(string userId, BagCreateInputModel model)
        {
            var loggedUser = await userManager.FindByIdAsync(userId);
            if (loggedUser == null || !await userManager.IsInRoleAsync(loggedUser, "Admin"))
                return false;

            string brandBg = TextUtility.CapitalizeNames(TextUtility.TransliterateToBg(model.Brand));
            string modelBg = TextUtility.CapitalizeNames(TextUtility.TransliterateToBg(model.Model));

            var bag = new Bag
            {
                Brand = model.Brand,
                BrandBg = brandBg,
                Model = model.Model,
                ModelBg = modelBg,
                Price = model.Price,
                Quantity = model.Quantity,
                ImageUrl = model.ImageUrl ?? "~/pictures/DefaultBagImage.webp",
            };

            await dbContext.Bags.AddAsync(bag);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<BagEditFormModel> GetBagForEditingAsync(string userId, int? id)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool IsAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (!IsAdmin || userId == null)
            {
                throw new ArgumentException("You do not have permission to edit bags.");
            }

            var bag = await FindBagByIdAsync(id);

            var model = new BagEditFormModel
            {
                Id = bag.Id,
                Brand = bag.Brand,
                Model = bag.Model,
                Price = bag.Price,
                Quantity = bag.Quantity,
                ImageUrl = bag.ImageUrl
            };

            return model;
        }

        public async Task<bool> EditBagAsync(BagEditFormModel model)
        {
            var bag = await dbContext.Bags.FirstOrDefaultAsync(b => b.Id == model.Id);
            if (bag == null) throw new ArgumentException(BagNotFoundErrorMessage);

            string brandBg = TextUtility.CapitalizeNames(TextUtility.TransliterateToBg(model.Brand));
            string modelBg = TextUtility.CapitalizeNames(TextUtility.TransliterateToBg(model.Model));

            bag.Brand = model.Brand;
            bag.BrandBg = brandBg;
            bag.Model = model.Model;
            bag.ModelBg = modelBg;
            bag.Price = model.Price;
            bag.Quantity = model.Quantity;
            bag.ImageUrl = model.ImageUrl;

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BagDeleteViewModel> GetBagForDeletingAsync(string userId, int? id)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool IsAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (!IsAdmin || userId == null)
            {
                throw new ArgumentException("You do not have permission to delete bags.");
            }

            var bag = await FindBagByIdAsync(id);

            var currentCulture = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            bool isBg = currentCulture.Equals("bg", StringComparison.OrdinalIgnoreCase);

            var model = new BagDeleteViewModel
            {
                Id = bag.Id,
                Brand = isBg ? bag.BrandBg : bag.Brand,
                Model = isBg ? bag.ModelBg : bag.Model,
                ImageUrl = bag.ImageUrl
            };

            return model;
        }

        public async Task<bool> DeleteBagAsync(string userId, BagDeleteViewModel model)
        {
            var user = await userManager.FindByIdAsync(userId);

            var bag = await dbContext.Bags.FindAsync(model.Id);

            if (bag == null)
            {
                throw new ArgumentException(BagNotFoundErrorMessage);
            }

            dbContext.Bags.Remove(bag);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
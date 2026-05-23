using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TennisAcademyApp.Data.Models;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants;

namespace TennisAcademyApp.Data.Seeding
{
    public class RoleSeeding
    {
        public static async Task SeedIdentityAsync(IServiceProvider serviceProvider)
        {
            await SeedRolesAsync(serviceProvider);
            await SeedAdminAsync(serviceProvider);
            await SeedCoachesAsync(serviceProvider);
        }

        private static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            string[] roles = { Admin, Trainer, User };
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role {role}");
                    }
                }
            }
        }
        private static async Task SeedCoachesAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<TennisAcademyDbContext>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var coachesWithoutAccounts = await dbContext.Coaches
                .Where(c => c.UserId == null)
                .ToListAsync();

            string defaultPassword = configuration["CoachSettings:DefaultPassword"];

            if (string.IsNullOrEmpty(defaultPassword))
            {
                throw new InvalidOperationException("Default coach password is not configured in user secrets.");
            }

            foreach (var coach in coachesWithoutAccounts)
            {
                string coachEmail = $"{coach.Name.Replace(" ", ".").ToLower()}@tennis.com"
                    .Replace("ö", "o")
                    .Replace("ä", "a")
                    .Replace("ü", "u");

                var user = await userManager.FindByEmailAsync(coachEmail);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        Email = coachEmail,
                        UserName = coachEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, defaultPassword);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create Identity User for coach {coach.Name}");
                    }
                }

                if (!await userManager.IsInRoleAsync(user, Trainer))
                {
                    await userManager.AddToRoleAsync(user, Trainer);
                }

                coach.UserId = user.Id;
            }

            if (coachesWithoutAccounts.Any())
            {
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminConfiguration = serviceProvider.GetRequiredService<IConfiguration>();

            string adminUserEmail = adminConfiguration["AdminSettings:Username"];
            string adminUserPassword = adminConfiguration["AdminSettings:Password"];

            var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = adminUserEmail,
                    UserName = adminUserEmail,
                    EmailConfirmed = false
                };

                var result = await userManager.CreateAsync(adminUser, adminUserPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create user {adminUserEmail}");
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, Admin))
            {
                var result = await userManager.AddToRoleAsync(adminUser, Admin);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to add {adminUserEmail} to Admin role");
                }
            }
        }
    }
}
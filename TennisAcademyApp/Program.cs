using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Data.Models;
using TennisAcademyApp.Services;
using TennisAcademyApp.Services.Contracts;
using TennisAcademyApp.Services.Core;
using TennisAcademyApp.Services.Core.Contracts;
using static TennisAcademyApp.Data.Seeding.RoleSeeding;

namespace TennisAcademyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            builder.Services.AddDbContext<TennisAcademyDbContext>(options =>
                options.UseNpgsql(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<TennisAcademyDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews(cfg =>
            {
                cfg.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddScoped<ICoachService, CoachService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<ISurfaceService, SurfaceService>();
            builder.Services.AddScoped<ITrainingTypeService, TrainingTypeService>();
            builder.Services.AddScoped<IRacketService, RacketService>();
            builder.Services.AddScoped<IRacketCartService, RacketCartService>();
            builder.Services.AddScoped<IFavouriteCoachService, FavouriteCoachService>();
            builder.Services.AddScoped<IBallService, BallService>();
            builder.Services.AddScoped<IBallCartService, BallCartService>();
            builder.Services.AddScoped<IBagService, BagService>();
            builder.Services.AddScoped<IBagCartService, BagCartService>();
            builder.Services.AddScoped<ITournamentService, TournamentService>();
            builder.Services.AddScoped<IRankingService, RankingService>();
            builder.Services.AddScoped<IWheelService, WheelService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            builder.Services.AddSingleton(new DeepL.Translator(builder.Configuration["DeepL:ApiKey"]));

            var app = builder.Build(); // <--- Container is locked here

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = scope.ServiceProvider.GetRequiredService<TennisAcademyDbContext>();
                dbContext.Database.Migrate();
                SeedIdentityAsync(services).GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure Localization Middleware
            var supportedCultures = new[] { "en", "bg" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
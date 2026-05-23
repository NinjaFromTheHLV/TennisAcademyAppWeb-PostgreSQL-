
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TennisAcademyApp.Data;
using TennisAcademyApp.Services.Core;
using TennisAcademyApp.Services.Core.Contracts;

namespace TennisAcademyApp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<TennisAcademyDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TennisAcademyDbContext>();
            // Add services to the container.
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.WithOrigins("https://localhost:7140")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAllOrigins");

            app.UseAuthorization();
            app.MapIdentityApi<IdentityUser>();


            app.MapControllers();

            app.Run();
        }
    }
}

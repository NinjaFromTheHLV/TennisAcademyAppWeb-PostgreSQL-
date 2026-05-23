using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Seeding
{
    public class SurfaceSeeding : IEntityTypeConfiguration<Surface>
    {
        public void Configure(EntityTypeBuilder<Surface> config)
        {
            config.HasData(
            new Surface
            {
                Id = 1,
                Name = "Clay",
                NameBg = "Клей (Червен корт)",
                ImageUrl = "https://www.edwardssports.co.uk/pub/media/magefan_blog/Clay_Tennis_Courts.jpg"
            },
            new Surface
            {
                Id = 2,
                Name = "Grass",
                NameBg = "Трева",
                ImageUrl = "https://www.tennisnerd.net/wp-content/uploads/2024/06/grass-tennis.webp"
            },
            new Surface
            {
                Id = 3,
                Name = "Hard",
                NameBg = "Твърда настилка (Хард корт)",
                ImageUrl = "https://asltenniscourts.com.au/wp-content/uploads/2021/03/AdobeStock_253105355-1024x683.jpeg"
            });
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Seeding
{
    public class TrainingTypeSeeding : IEntityTypeConfiguration<TrainingType>
    {
        public void Configure(EntityTypeBuilder<TrainingType> config)
        {
            config.HasData(
                new TrainingType
                {
                    Id = 1,
                    Name = "Physical Conditioning Routine",
                    NameBg = "Физическа подготовка"
                },
                new TrainingType
                {
                    Id = 2,
                    Name = "Technical Skill Development",
                    NameBg = "Развитие на технически умения"
                },
                new TrainingType
                {
                    Id = 3,
                    Name = "Tactical Game Strategy",
                    NameBg = "Тактическа стратегия за игра"
                },
                new TrainingType
                {
                    Id = 4,
                    Name = "Mental Toughness Training",
                    NameBg = "Психологическа устойчивост и ментална тренировка"
                }
            );
        }
    }
}
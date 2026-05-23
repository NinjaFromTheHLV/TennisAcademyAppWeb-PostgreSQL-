using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Data.Seeding
{
    public class TournamentCategorySeeding : IEntityTypeConfiguration<TournamentCategory>
    {
        public void Configure(EntityTypeBuilder<TournamentCategory> builder)
        {
            builder.HasData(
                new TournamentCategory { Id = 1, Name = "Singles Men", NameBg = "Сингъл Мъже", IsDeleted = false },
                new TournamentCategory { Id = 2, Name = "Singles Women", NameBg = "Сингъл Жени", IsDeleted = false },
                new TournamentCategory { Id = 3, Name = "Juniors", NameBg = "Юноши", IsDeleted = false },
                new TournamentCategory { Id = 4, Name = "Doubles Mixed", NameBg = "Смесени Двойки", IsDeleted = false },
                new TournamentCategory { Id = 5, Name = "Veterans 45+", NameBg = "Ветерани 45+", IsDeleted = false },
                new TournamentCategory { Id = 6, Name = "Amateur League", NameBg = "Аматьорска Лига", IsDeleted = false }
            );
        }
    }
}
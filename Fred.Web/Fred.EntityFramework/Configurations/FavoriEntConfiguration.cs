using Fred.Entities.Favori;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FavoriEntConfiguration : IEntityTypeConfiguration<FavoriEnt>
    {
        public void Configure(EntityTypeBuilder<FavoriEnt> builder)
        {
            builder.ToTable("FRED_FAVORI_UTILISATEUR");
            builder.HasKey(x => x.FavoriId);

            builder.Property(x => x.FavoriId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Libelle)
                .IsUnicode(false)
                .HasMaxLength(100);
            builder.Property(x => x.Couleur)
                .IsUnicode(false)
                .HasMaxLength(10);
            builder.Property(x => x.TypeFavori)
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(x => x.UrlFavori)
                .IsUnicode(false)
                .HasMaxLength(250);
            builder.Property(x => x.Search)
                .HasColumnType("varbinary(max)");

            builder.HasOne(x => x.Utilisateur)
                .WithMany()
                .HasForeignKey(x => x.UtilisateurId);
        }
    }
}


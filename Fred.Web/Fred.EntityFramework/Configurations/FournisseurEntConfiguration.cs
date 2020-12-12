using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FournisseurEntConfiguration : IEntityTypeConfiguration<FournisseurEnt>
    {
        public void Configure(EntityTypeBuilder<FournisseurEnt> builder)
        {
            builder.ToTable("FRED_FOURNISSEUR");
            builder.HasKey(f => f.FournisseurId);

            builder.Property(f => f.FournisseurId)
                .ValueGeneratedOnAdd();
            builder.Property(f => f.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(f => f.Libelle)
                .HasMaxLength(500);
            builder.Property(f => f.Adresse)
                .HasMaxLength(250);
            builder.Property(f => f.CodePostal)
                .HasMaxLength(10);
            builder.Property(f => f.Ville)
                .HasMaxLength(50);
            builder.Property(f => f.SIRET)
                .HasMaxLength(19);
            builder.Property(f => f.SIREN)
                .HasMaxLength(19);
            builder.Property(f => f.Telephone)
                .HasMaxLength(15);
            builder.Property(f => f.Fax)
                .HasMaxLength(15);
            builder.Property(f => f.Email)
                .HasMaxLength(250);
            builder.Property(f => f.ModeReglement)
                .HasMaxLength(255);
            builder.Property(f => f.TypeSequence)
                .HasMaxLength(255);
            builder.Property(f => f.CodeTVA)
                .HasMaxLength(19);
            builder.Property(f => f.TypeTiers)
                .IsFixedLength()
                .HasMaxLength(1);

            builder.Property(cod => cod.DateOuverture)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCloture)
                .HasColumnType("datetime");
            builder.Property(f => f.IsProfessionLiberale)
                .HasDefaultValue(0);

            builder.Property(f => f.BlocageContratInterimaireManuel)
                .HasDefaultValue(0);

            builder.Property(f => f.ReferenceSystemeInterimaire)
                .HasMaxLength(150);

            builder.HasOne(f => f.Groupe)
                .WithMany(g => g.Fournisseurs)
                .HasForeignKey(f => f.GroupeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.Pays)
                .WithMany()
                .HasForeignKey(f => f.PaysId);
        }
    }
}


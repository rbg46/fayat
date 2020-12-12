using Fred.Entities.Personnel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PersonnelEntConfiguration : IEntityTypeConfiguration<PersonnelEnt>
    {
        public void Configure(EntityTypeBuilder<PersonnelEnt> builder)
        {
            builder.ToTable("FRED_PERSONNEL");
            builder.HasKey(p => p.PersonnelId);
            builder.HasIndex(p => new { p.Matricule, p.SocieteId })
                .HasName("IX_UniqueMatriculeAndSociete")
                .IsUnique()
                .HasFilter(null);
            builder.HasIndex(p => p.Email)
                .HasName("IX_UniqueEmail")
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL AND [IsInterne]=(0)");

            builder.HasIndex(p => new { p.Nom, p.Prenom, p.Matricule });

            builder.Property(p => p.PersonnelId)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Matricule)
                .HasMaxLength(10);
            builder.Property(p => p.Nom)
                .HasMaxLength(150);
            builder.Property(p => p.Prenom)
                .HasMaxLength(150);
            builder.Property(p => p.Statut)
                .HasMaxLength(1);
            builder.Property(p => p.CategoriePerso)
                .HasMaxLength(1);
            builder.Property(p => p.Adresse1)
                .HasMaxLength(250);
            builder.Property(p => p.Adresse2)
                .HasMaxLength(250);
            builder.Property(p => p.Adresse3)
                .HasMaxLength(250);
            builder.Property(p => p.CodePostal)
                .HasMaxLength(20);
            builder.Property(p => p.Ville)
                .HasMaxLength(250);
            builder.Property(p => p.PaysLabel)
                .HasMaxLength(150);
            builder.Property(p => p.Telephone1)
                .HasColumnName(@"Tel1")
                .HasMaxLength(50);
            builder.Property(p => p.Telephone2)
                .HasColumnName(@"Tel2")
                .HasMaxLength(50);
            builder.Property(p => p.Email)
                .HasMaxLength(50);
            builder.Property(p => p.TypeRattachement)
                .HasColumnType("char")
                .IsFixedLength()
                .IsUnicode(false)
                .HasMaxLength(1);
            builder.Property(p => p.DateCreation)
                .IsRequired()
                .HasDefaultValueSql("getdate()");
            builder.Property(p => p.EtablissementPaieId)
                .HasColumnName(@"EtablissementPayeId");

            builder.Property(cod => cod.DateEntree)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSortie)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");
            builder.Property(cod => cod.TimestampImport)
                .HasColumnType("bigint");
            
            builder.Ignore(p => p.UtilisateurId);
            builder.Ignore(p => p.NomPrenom);
            builder.Ignore(p => p.PrenomNom);
            builder.Ignore(p => p.CodeNomPrenom);
            builder.Ignore(p => p.Adresse);
            builder.Ignore(p => p.ContratActif);

            builder.HasOne(p => p.Societe)
                .WithMany(s => s.Personnels)
                .HasForeignKey(p => p.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Ressource)
                .WithMany(r => r.Personnels)
                .HasForeignKey(p => p.RessourceId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Materiel)
                .WithMany()
                .HasForeignKey(p => p.MaterielId);
            builder.HasOne(p => p.EtablissementPaie)
                .WithMany()
                .HasForeignKey(p => p.EtablissementPaieId);
            builder.HasOne(p => p.Pays)
                .WithMany(p => p.Personnels)
                .HasForeignKey(p => p.PaysId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.EtablissementRattachement)
                .WithMany()
                .HasForeignKey(p => p.EtablissementRattachementId);
            builder.HasOne(p => p.Equipe)
                .WithMany()
                .HasForeignKey(p => p.EquipeFavoriteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.PersonnelImage)
                .WithMany()
                .HasForeignKey(p => p.PersonnelImageId);
            builder.HasOne(p => p.Manager)
                .WithMany(m => m.ManagerPersonnels)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

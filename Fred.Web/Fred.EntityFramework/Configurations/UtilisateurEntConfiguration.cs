using Fred.Entities.Utilisateur;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class UtilisateurEntConfiguration : IEntityTypeConfiguration<UtilisateurEnt>
    {
        public void Configure(EntityTypeBuilder<UtilisateurEnt> builder)
        {
            builder.ToTable("FRED_UTILISATEUR");
            builder.HasKey(u => u.UtilisateurId);
            builder.Property(u => u.UtilisateurId)
                .ValueGeneratedNever();
            builder.HasIndex(u => u.FayatAccessDirectoryId)
                .HasName("IX_FayatAccessDirectoryId")
                .IsUnique();
            builder.HasIndex(u => u.Login)
                .HasName("IX_UniqueLogin")
                .IsUnique();

            builder.Property(cod => cod.DateDerniereConnexion)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSupression)
                .HasColumnType("datetime");

            builder.Property(cod => cod.SuperAdmin)
                .HasDefaultValue(0);
            builder.Property(cod => cod.CommandeManuelleAllowed)
                .HasDefaultValue(0);

            builder.Ignore(u => u.PersonnelId);
            builder.Ignore(u => u.Email);
            builder.Ignore(u => u.Nom);
            builder.Ignore(u => u.Prenom);
            builder.Ignore(u => u.PrenomNom);
            builder.Property(u => u.Login)
                .HasMaxLength(50);
            builder.Property(u => u.Folio)
                .HasMaxLength(10);

            builder.HasOne(u => u.Personnel)
                .WithOne(p => p.Utilisateur)
                .HasForeignKey<UtilisateurEnt>(u => u.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(u => u.ExternalDirectory)
                .WithMany(ed => ed.Utilisateurs)
                .HasForeignKey(u => u.FayatAccessDirectoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(u => u.AuteurCreation)
                .WithMany()
                .HasForeignKey(u => u.UtilisateurIdCreation);
            builder.HasOne(u => u.AuteurSuppression)
                .WithMany()
                .HasForeignKey(u => u.UtilisateurIdSupression);
            builder.HasOne(u => u.AuteurModification)
                .WithMany()
                .HasForeignKey(u => u.UtilisateurIdModification);
        }
    }
}

using Fred.Entities.Personnel.Interimaire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ContratInterimaireEntConfiguration : IEntityTypeConfiguration<ContratInterimaireEnt>
    {
        public void Configure(EntityTypeBuilder<ContratInterimaireEnt> builder)
        {
            builder.ToTable("FRED_CONTRAT_INTERIMAIRE");
            builder.HasKey(ci => ci.ContratInterimaireId);

            builder.Property(ci => ci.ContratInterimaireId)
                .HasColumnName("PersonnelFournisseurSocieteId")
                .ValueGeneratedOnAdd();
            builder.Property(ci => ci.NumContrat)
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(ci => ci.Source)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(ci => ci.Qualification)
                .HasMaxLength(150);
            builder.Property(ci => ci.Commentaire)
                .HasMaxLength(500);

            builder.Property(ci => ci.DateDebut)
                .HasColumnType("datetime");
            builder.Property(ci => ci.DateFin)
                .HasColumnType("datetime");
            builder.Property(ci => ci.Valorisation)
                .HasDefaultValue((decimal)0);

            builder.HasOne(x => x.Interimaire).WithMany(x => x.ContratInterimaires).HasForeignKey(x => x.InterimaireId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Fournisseur).WithMany(x => x.ContratInterimaires).HasForeignKey(x => x.FournisseurId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Societe).WithMany(x => x.ContratInterimaires).HasForeignKey(x => x.SocieteId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Ci).WithMany().HasForeignKey(x => x.CiId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Ressource).WithMany().HasForeignKey(x => x.RessourceId);
            builder.HasOne(x => x.Unite).WithMany().HasForeignKey(x => x.UniteId);
            builder.HasOne(x => x.MotifRemplacement).WithMany().HasForeignKey(x => x.MotifRemplacementId);
            builder.HasOne(x => x.PersonnelRemplace).WithMany().HasForeignKey(x => x.PersonnelRemplaceId);
            builder.HasOne(x => x.EtatContrat).WithMany(e=>e.ContratInterimaires).HasForeignKey(x => x.EtatContratId);
        }
    }
}


using Fred.Entities.Facturation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FacturationEntConfiguration : IEntityTypeConfiguration<FacturationEnt>
    {
        public void Configure(EntityTypeBuilder<FacturationEnt> builder)
        {
            builder.ToTable("FRED_FACTURATION");
            builder.HasKey(f => f.FacturationId);
            builder.Property(f => f.FacturationId)
                .ValueGeneratedOnAdd();

            builder.Ignore(f => f.CodeCi);
            builder.Ignore(f => f.SocieteCode);
            builder.Ignore(f => f.PuFacture);

            builder.Property(f => f.EcartPu)
                .HasColumnType("numeric(18, 3)");
            builder.Property(f => f.Quantite)
                .HasColumnType("numeric(18, 3)");
            builder.Property(f => f.EcartQuantite)
                .HasColumnType("numeric(18, 3)");
            builder.Property(f => f.QuantiteReconduite)
                .HasColumnType("numeric(18, 3)");
            builder.Property(f => f.MouvementFarHt)
                .HasColumnType("numeric(18, 3)");
            builder.Property(f => f.TotalFarHt)
                .HasColumnType("numeric(18, 3)");
            builder.Property(f => f.QuantiteFar)
                .HasColumnType("numeric(18, 3)");

            builder.Property(f => f.DatePieceSap)
                .HasColumnType("datetime");
            builder.Property(f => f.DateSaisie)
                .HasColumnType("datetime");
            builder.Property(f => f.DateComptable)
                .HasColumnType("datetime");
            builder.Property(f => f.DateCreation)
                .HasColumnType("datetime");

            builder.Property(f => f.MontantTotalHT)
                .HasDefaultValue(0);

            builder.HasOne(f => f.FacturationType)
                .WithMany(c => c.Facturations)
                .HasForeignKey(f => f.FacturationTypeId);
            builder.HasOne(f => f.Commande)
                .WithMany(c => c.Facturations)
                .HasForeignKey(f => f.CommandeId);
            builder.HasOne(f => f.DepenseAchatReception)
                .WithMany(da => da.FacturationsReception)
                .HasForeignKey(f => f.DepenseAchatReceptionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.DepenseAchatFactureEcart)
                .WithMany(da => da.FacturationsFactureEcart)
                .HasForeignKey(f => f.DepenseAchatFactureEcartId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.DepenseAchatFacture)
                .WithMany(da => da.FacturationsFacture)
                .HasForeignKey(f => f.DepenseAchatFactureId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.DepenseAchatFar)
                .WithMany(da => da.FacturationsFar)
                .HasForeignKey(f => f.DepenseAchatFarId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.Devise)
                .WithMany(d => d.Facturations)
                .HasForeignKey(f => f.DeviseId);
            builder.HasOne(f => f.CI)
                .WithMany()
                .HasForeignKey(f => f.CiId);
            builder.HasOne(f => f.DepenseAchatAjustement)
                .WithMany()
                .HasForeignKey(f => f.DepenseAchatAjustementId);
        }
    }
}

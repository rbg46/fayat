using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TacheEntConfiguration : IEntityTypeConfiguration<TacheEnt>
    {
        public void Configure(EntityTypeBuilder<TacheEnt> builder)
        {
            builder.ToTable("FRED_TACHE");
            builder.HasKey(t => t.TacheId);
            builder.Property(t => t.TacheId)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(15);
            builder.Property(t => t.Libelle)
                .IsRequired()
                .HasMaxLength(255);
            builder.Ignore(t => t.CodeLibelle);
            builder.Property(t => t.Niveau)
                .IsRequired();
            builder.Property(t => t.QuantiteARealise)
                .HasColumnType("float");

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");

            builder.Property(t => t.TacheType)
                .HasDefaultValue(999999);

            builder.HasOne(t => t.CI)
                .WithMany(c => c.Taches)
                .HasForeignKey(t => t.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Parent)
                .WithMany(t => t.TachesEnfants)
                .HasForeignKey(t => t.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Budget)
                .WithMany()
                .HasForeignKey(t => t.BudgetId);
            builder.HasOne(t => t.AuteurCreation)
                .WithMany()
                .HasForeignKey(t => t.AuteurCreationId);
            builder.HasOne(t => t.AuteurModification)
                .WithMany()
                .HasForeignKey(t => t.AuteurModificationId);
            builder.HasOne(t => t.AuteurSuppression)
                .WithMany()
                .HasForeignKey(t => t.AuteurSuppressionId);
        }
    }
}

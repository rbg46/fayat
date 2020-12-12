using Fred.Entities.Budget.Avancement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AvancementEntConfiguration : IEntityTypeConfiguration<AvancementEnt>
    {
        public void Configure(EntityTypeBuilder<AvancementEnt> builder)
        {
            builder.ToTable("FRED_AVANCEMENT");
            builder.HasKey(a => a.AvancementId);
            builder.Property(a => a.AvancementId)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.QuantiteSousDetailAvancee)
                .HasColumnType("decimal(20, 8)");
            builder.Property(a => a.DAD)
                .HasColumnType("decimal(20, 8)");
            builder.Property(a => a.Periode)
                .HasDefaultValue(0);

            builder.HasOne(a => a.BudgetSousDetail)
                .WithMany()
                .HasForeignKey(a => a.BudgetSousDetailId);
            builder.HasOne(a => a.Ci)
                .WithMany()
                .HasForeignKey(a => a.CiId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Devise)
                .WithMany()
                .HasForeignKey(a => a.DeviseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.AvancementEtat)
                .WithMany()
                .HasForeignKey(a => a.AvancementEtatId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

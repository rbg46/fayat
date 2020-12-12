using Fred.Entities.Budget.Avancement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AvancementWorkflowEntConfiguration : IEntityTypeConfiguration<AvancementWorkflowEnt>
    {
        public void Configure(EntityTypeBuilder<AvancementWorkflowEnt> builder)
        {
            builder.ToTable("FRED_AVANCEMENT_WORKFLOW");
            builder.HasKey(aw => aw.AvancementWorkflowId);
            builder.Property(aw => aw.AvancementWorkflowId)
                .ValueGeneratedOnAdd();

            builder.Property(aw => aw.Date)
                .HasColumnType("datetime");

            builder.HasOne(aw => aw.Avancement)
                .WithMany(a => a.Workflows)
                .HasForeignKey(aw => aw.AvancementId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(aw => aw.EtatInitial)
                .WithMany()
                .HasForeignKey(aw => aw.EtatInitialId);
            builder.HasOne(aw => aw.EtatCible)
                .WithMany()
                .HasForeignKey(aw => aw.EtatCibleId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(aw => aw.Auteur)
                .WithMany()
                .HasForeignKey(aw => aw.AuteurId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

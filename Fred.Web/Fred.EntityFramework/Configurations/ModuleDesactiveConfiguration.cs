using Fred.Entities.ModuleDesactive;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class ModuleDesactiveConfiguration : IEntityTypeConfiguration<ModuleDesactiveEnt>
    {
        public void Configure(EntityTypeBuilder<ModuleDesactiveEnt> builder)
        {
            builder.ToTable("FRED_MODULE_DESACTIVE");
            builder.HasKey(md => md.ModuleDesactiveId);
            builder.Property(md => md.ModuleDesactiveId)
                .ValueGeneratedOnAdd();

            builder.HasOne(md => md.Societe)
                .WithMany()
                .HasForeignKey(md => md.SocieteId);
            builder.HasOne(md => md.Module)
                .WithMany()
                .HasForeignKey(md => md.ModuleId);
        }
    }
}

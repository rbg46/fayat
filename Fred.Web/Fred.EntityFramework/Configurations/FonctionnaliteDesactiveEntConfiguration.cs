using Fred.Entities.FonctionnaliteDesactive;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class FonctionnaliteDesactiveEntConfiguration : IEntityTypeConfiguration<FonctionnaliteDesactiveEnt>
    {
        public void Configure(EntityTypeBuilder<FonctionnaliteDesactiveEnt> builder)
        {
            builder.ToTable("FRED_FONCTIONNALITE_DESACTIVE");
            builder.HasKey(fd => fd.FonctionnaliteDesactiveId);
            builder.Property(fd => fd.FonctionnaliteDesactiveId)
                .ValueGeneratedOnAdd();

            builder.HasOne(fod => fod.Societe)
                .WithMany()
                .HasForeignKey(fod => fod.SocieteId);
            builder.HasOne(fod => fod.Fonctionnalite)
                .WithMany()
                .HasForeignKey(fod => fod.FonctionnaliteId);
        }
    }
}

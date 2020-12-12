
using Fred.Entities.Commande;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CommandeTypeEntConfiguration : IEntityTypeConfiguration<CommandeTypeEnt>
    {
        public void Configure(EntityTypeBuilder<CommandeTypeEnt> builder)
        {
            builder.ToTable("FRED_COMMANDE_TYPE");
            builder.HasKey(ct => ct.CommandeTypeId);
            builder.HasIndex(ct => ct.Code)
                .HasName("IX_UniqueCode")
                .IsUnique();

            builder.Property(ct => ct.CommandeTypeId)
                .ValueGeneratedOnAdd();
            builder.Property(ct => ct.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(ct => ct.Libelle)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}


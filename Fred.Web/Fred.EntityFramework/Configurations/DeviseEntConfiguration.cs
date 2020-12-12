
using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class DeviseEntConfiguration : IEntityTypeConfiguration<DeviseEnt>
    {
        public void Configure(EntityTypeBuilder<DeviseEnt> builder)
        {
            builder.ToTable("FRED_DEVISE");
            builder.HasKey(x => x.DeviseId);
            builder.HasIndex(x => x.IsoCode)
                .HasName("IX_UniqueIsoCode")
                .IsUnique()
                .HasFilter(null);

            builder.Property(x => x.DeviseId)
                .ValueGeneratedOnAdd();
            builder.Ignore(x => x.Reference);

            builder.Property(x => x.IsoCode)
                .HasMaxLength(3);
            builder.Property(x => x.IsoNombre)
                .HasMaxLength(5);
            builder.Property(x => x.Symbole)
                .HasMaxLength(10);
            builder.Property(x => x.CodeHtml)
                .HasMaxLength(10);
            builder.Property(x => x.Libelle)
                .HasMaxLength(150);
            builder.Property(x => x.CodePaysIso)
                .HasColumnType("nvarchar(2)");

            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateSuppression)
                .HasColumnType("datetime");
        }
    }
}


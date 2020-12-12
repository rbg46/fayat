using Fred.Entities.ReferentielFixe;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TypeRessourceEntConfiguration : IEntityTypeConfiguration<TypeRessourceEnt>
    {
        public void Configure(EntityTypeBuilder<TypeRessourceEnt> builder)
        {
            builder.ToTable("FRED_TYPE_RESSOURCE");
            builder.HasKey(tr => tr.TypeRessourceId);
            builder.Property(tr => tr.TypeRessourceId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(tr => tr.Code)
                .HasName("IX_UniqueCode")
                .IsUnique();

            builder.Property(tr => tr.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(tr => tr.Libelle)
                .IsRequired()
                .HasMaxLength(500);

            builder.Ignore(tr => tr.CodeLibelle);
        }
    }
}
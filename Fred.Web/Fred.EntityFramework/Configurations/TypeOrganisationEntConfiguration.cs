using Fred.Entities.Organisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TypeOrganisationEntConfiguration : IEntityTypeConfiguration<TypeOrganisationEnt>
    {
        public void Configure(EntityTypeBuilder<TypeOrganisationEnt> builder)
        {
            builder.ToTable("FRED_TYPE_ORGANISATION");
            builder.HasKey(to => to.TypeOrganisationId);
            builder.Property(to => to.TypeOrganisationId)
                .ValueGeneratedOnAdd();
            builder.HasIndex(to => to.Code)
                .HasName("IX_UniqueTypeOrganisationCode")
                .IsUnique();

            builder.Property(to => to.Code)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(to => to.Libelle)
                .IsRequired()
                .HasMaxLength(500);

            builder.Ignore(to => to.CodeLibelle);
        }
    }
}

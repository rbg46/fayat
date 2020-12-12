using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class TypeParticipationSepEntConfiguration : IEntityTypeConfiguration<TypeParticipationSepEnt>
    {
        public void Configure(EntityTypeBuilder<TypeParticipationSepEnt> builder)
        {
            builder.ToTable("FRED_TYPE_PARTICIPATION_SEP");
            builder.HasKey(tps => tps.TypeParticipationSepId);
            builder.Property(tps => tps.TypeParticipationSepId)
                .ValueGeneratedOnAdd();

            builder.Property(tps => tps.Code)
                .IsRequired();
            builder.Property(tps => tps.Libelle)
                .IsRequired();
        }
    }
}

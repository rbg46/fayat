using Fred.Entities.Depense;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class GroupeRemplacementTacheEntConfiguration : IEntityTypeConfiguration<GroupeRemplacementTacheEnt>
    {
        public void Configure(EntityTypeBuilder<GroupeRemplacementTacheEnt> builder)
        {
            builder.ToTable("FRED_GROUPE_REMPLACEMENT_TACHE");
            builder.HasKey(grt => grt.GroupeRemplacementTacheId);
            builder.Property(grt => grt.GroupeRemplacementTacheId)
                .ValueGeneratedOnAdd();
        }
    }
}

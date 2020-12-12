using Fred.Entities.Commande;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class ActionCommandeLigneEntConfiguration : IEntityTypeConfiguration<ActionCommandeLigneEnt>
    {
        public void Configure(EntityTypeBuilder<ActionCommandeLigneEnt> builder)
        {
            builder.ToTable("FRED_ACTION_COMMANDE_LIGNE");
            builder.HasKey(acl => acl.ActionCommandeLigneId);
            builder.HasIndex(acl => acl.ActionCommandeLigneId);
            builder.Property(acl => acl.ActionCommandeLigneId)
                .ValueGeneratedOnAdd();

            builder.HasOne(c => c.CommandeLigne)
                .WithMany()
                .HasForeignKey(c => c.CommandeLigneId);
            builder.HasOne(c => c.Action)
                .WithMany()
                .HasForeignKey(c => c.ActionId);
        }
    }
}

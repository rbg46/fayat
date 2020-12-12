using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class AssocieSepConfiguration : IEntityTypeConfiguration<AssocieSepEnt>
    {
        public void Configure(EntityTypeBuilder<AssocieSepEnt> builder)
        {
            builder.ToTable("FRED_ASSOCIE_SEP");
            builder.HasKey(@as => @as.AssocieSepId);

            builder.Property(@as => @as.QuotePart)
                .HasColumnType("numeric(12, 2)");

            builder.Property(@as => @as.AssocieSepId)
                .ValueGeneratedOnAdd();
            builder.HasOne(@as => @as.Societe)
                .WithMany(s => s.AssocieSeps)
                .HasForeignKey(@as => @as.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(@as => @as.SocieteAssociee)
                .WithMany()
                .HasForeignKey(@as => @as.SocieteAssocieeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(@as => @as.TypeParticipationSep)
                .WithMany()
                .HasForeignKey(@as => @as.TypeParticipationSepId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(@as => @as.Fournisseur)
                .WithMany()
                .HasForeignKey(@as => @as.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(@as => @as.AssocieSepParent)
                .WithMany(@as => @as.AssocieSepChildren)
                .HasForeignKey(@as => @as.AssocieSepParentId);
        }
    }
}

using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PieceJointeEntConfiguration : AuditableEntityConfiguration<PieceJointeEnt>, IEntityTypeConfiguration<PieceJointeEnt>
    {
        public new void Configure(EntityTypeBuilder<PieceJointeEnt> builder)
        {
            base.Configure(builder);

            builder.ToTable("FRED_PIECE_JOINTE");
            builder.HasKey(pj => pj.PieceJointeId);
            builder.Property(pj => pj.PieceJointeId)
                .ValueGeneratedOnAdd();

            builder.Ignore(pj => pj.PieceJointeArray); 

            builder.Property(pj => pj.Libelle)
                .HasMaxLength(250);
            builder.Property(pj => pj.RelativePath)
                .HasColumnName("Url")
                .IsRequired();
            builder.Property(pj => pj.DateCreation)
                .HasColumnType("datetime");
            builder.Property(pj => pj.DateModification)
                .HasColumnType("datetime");
            builder.Property(pj => pj.SizeInKo)
                .HasDefaultValue(0);
        }
    }
}

using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PieceJointeReceptionEntConfiguration : AuditableEntityConfiguration<PieceJointeReceptionEnt>, IEntityTypeConfiguration<PieceJointeReceptionEnt>
    {
        public new void Configure(EntityTypeBuilder<PieceJointeReceptionEnt> builder)
        {
            base.Configure(builder);

            builder.ToTable("FRED_PIECE_JOINTE_RECEPTION");
            builder.HasKey(pjr => pjr.PieceJointeReceptionId);
            builder.Property(pjr => pjr.PieceJointeReceptionId)
                .ValueGeneratedOnAdd();

            builder.HasOne(pjr => pjr.PieceJointe)
                .WithMany()
                .HasForeignKey(pjr => pjr.PieceJointeId);
            builder.HasOne(pjr => pjr.Reception)
                .WithMany(pjr => pjr.PiecesJointesReception)
                .HasForeignKey(pjr => pjr.ReceptionId);
            builder.Property(pjr => pjr.DateCreation)
                .HasColumnType("datetime");
            builder.Property(pjr => pjr.DateModification)
                .HasColumnType("datetime");
        }
    }
}

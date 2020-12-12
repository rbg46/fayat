using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PieceJointeCommandeEntConfiguration : AuditableEntityConfiguration<PieceJointeCommandeEnt>, IEntityTypeConfiguration<PieceJointeCommandeEnt>
    {
        public new void Configure(EntityTypeBuilder<PieceJointeCommandeEnt> builder)
        {
            base.Configure(builder);

            builder.ToTable("FRED_PIECE_JOINTE_COMMANDE");
            builder.HasKey(x => x.PieceJointeCommandeId);

            builder.Property(x => x.PieceJointeCommandeId)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.PieceJointe)
                .WithMany()
                .HasForeignKey(x => x.PieceJointeId);
            builder.HasOne(x => x.Commande)
                .WithMany(c => c.PiecesJointesCommande)
                .HasForeignKey(x => x.CommandeId);
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");
        }
    }
}

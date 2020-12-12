
using Fred.Entities.Referential;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class CodeZoneDeplacementEntConfiguration : IEntityTypeConfiguration<CodeZoneDeplacementEnt>
    {
        public void Configure(EntityTypeBuilder<CodeZoneDeplacementEnt> builder)
        {
            builder.ToTable("FRED_CODE_ZONE_DEPLACEMENT");
            builder.HasKey(czd => czd.CodeZoneDeplacementId);
            builder.HasIndex(czd => new { czd.Code, czd.SocieteId })
                .HasName("IX_UniqueCodeAndSociete")
                .IsUnique();

            builder.Property(czd => czd.CodeZoneDeplacementId)
                .ValueGeneratedOnAdd();
            builder.Property(czd => czd.Code)
                .HasMaxLength(20);
            builder.Property(czd => czd.Libelle)
                .IsUnicode(false)
                .HasMaxLength(500);
            builder.Property(czd => czd.DateCreation)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
            builder.Property(czd => czd.DateModification)
                .HasColumnType("datetime");
            builder.Property(czd => czd.KmMini)
                .HasDefaultValue(0);
            builder.Property(czd => czd.KmMaxi)
                .HasDefaultValue(0);

            builder.HasOne(czd => czd.Societe)
                .WithMany(s => s.CodeZoneDeplacements)
                .HasForeignKey(czd => czd.SocieteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(czd => czd.UtilisateurAuteurCreation)
               .WithMany()
               .HasForeignKey(czd => czd.AuteurCreation);
            builder.HasOne(czd => czd.UtilisateurAuteurModification)
               .WithMany()
               .HasForeignKey(czd => czd.AuteurModification);
        }
    }
}


using Fred.Entities.Personnel.Interimaire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class MatriculeExterneEntConfiguration : IEntityTypeConfiguration<MatriculeExterneEnt>
    {
        public void Configure(EntityTypeBuilder<MatriculeExterneEnt> builder)
        {
            builder.ToTable("FRED_MATRICULE_EXTERNE");
            builder.HasKey(me => me.MatriculeExterneId);
            builder.Property(me => me.MatriculeExterneId)
                .ValueGeneratedOnAdd();

            builder.HasIndex(me => new { me.PersonnelId, me.Source })
                .HasName("IX_UniquePersonnelIdAndSource")
                .IsUnique();
            builder.HasIndex(me => new { me.Matricule, me.Source })
                .HasName("IX_UniqueMatriculeAndSource")
                .IsUnique();

            builder.HasOne(me => me.Personnel)
                .WithMany(p => p.MatriculeExterne)
                .HasForeignKey(me => me.PersonnelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(me => me.Matricule).IsRequired().HasMaxLength(150);
            builder.Property(me => me.Source).IsRequired().HasMaxLength(150);
        }
    }
}

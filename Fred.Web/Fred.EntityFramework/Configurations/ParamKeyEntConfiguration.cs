using Fred.Entities.Params;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    class ParamKeyEntConfiguration : IEntityTypeConfiguration<ParamKeyEnt>
    {
        public void Configure(EntityTypeBuilder<ParamKeyEnt> builder)
        {
            builder.ToTable("FRED_PARAM_KEY");
            builder.HasKey(pk => pk.ParamKeyId);

            builder.Property(pk => pk.Libelle)
                .HasMaxLength(500);
            builder.Property(pk => pk.Description)
                .HasMaxLength(500);
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");

            builder.Property(pk => pk.ParamKeyId)
                .ValueGeneratedOnAdd();
            builder.HasOne(pk => pk.AuteurCreation)
                .WithMany()
                .HasForeignKey(pk => pk.AuteurCreationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pk => pk.AuteurModification)
                .WithMany()
                .HasForeignKey(pk => pk.AuteurModificationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

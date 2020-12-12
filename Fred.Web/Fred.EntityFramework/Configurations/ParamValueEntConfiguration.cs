using Fred.Entities.Params;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class ParamValueEntConfiguration : IEntityTypeConfiguration<ParamValueEnt>
    {
        public void Configure(EntityTypeBuilder<ParamValueEnt> builder)
        {
            builder.ToTable("FRED_PARAM_VALUE");
            builder.HasKey(pv => pv.ParamValueId);
            builder.Property(pv => pv.ParamValueId)
                .ValueGeneratedOnAdd();

            builder.Property(pv => pv.Valeur)
                .HasColumnName("Value")
                .HasMaxLength(500);
            builder.Property(cod => cod.DateCreation)
                .HasColumnType("datetime");
            builder.Property(cod => cod.DateModification)
                .HasColumnType("datetime");

            builder.HasOne(pv => pv.Organisation)
                .WithMany(o => o.ParamValues)
                .HasForeignKey(pv => pv.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pv => pv.ParamKey)
                .WithMany(pk => pk.ParamValues)
                .HasForeignKey(pv => pv.ParamKeyId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pv => pv.AuteurCreation)
                .WithMany()
                .HasForeignKey(pv => pv.AuteurCreationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pv => pv.AuteurModification)
                .WithMany()
                .HasForeignKey(pv => pv.AuteurModificationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

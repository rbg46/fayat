using Fred.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public abstract class AuditableEntityConfiguration<TEntity> where TEntity : AuditableEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(a => a.DateCreation)
                .HasColumnType("datetime");
            builder.Property(a => a.DateModification)
                .HasColumnType("datetime");

            builder.HasOne(a => a.AuteurCreation)
                .WithMany()
                .HasForeignKey(a => a.AuteurCreationId);
            builder.HasOne(a => a.AuteurModification)
                .WithMany()
                .HasForeignKey(a => a.AuteurModificationId);
        }
    }
}
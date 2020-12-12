using System;
using System.Collections.Generic;
using System.Text;
using Fred.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public abstract class DeletableConfiguration<TEntity> : AuditableEntityConfiguration<TEntity> where TEntity : Deletable
    {
        public new void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.Property(d => d.DateSuppression)
                .HasColumnType("datetime");

            builder.HasOne(a => a.AuteurSuppression)
                .WithMany()
                .HasForeignKey(a => a.AuteurSuppressionId);
        }
    }
}

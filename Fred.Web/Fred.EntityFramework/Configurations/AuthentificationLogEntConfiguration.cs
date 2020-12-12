using System;
using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework.Configurations
{
    public class AuthentificationLogEntConfiguration : IEntityTypeConfiguration<AuthentificationLogEnt>
    {
        public void Configure(EntityTypeBuilder<AuthentificationLogEnt> builder)
        {
            builder.ToTable("FRED_AUTHENTIFICATION_LOG");
            builder.HasKey(sbec => sbec.AuthentificationLogId);
            builder.Property(sbec => sbec.AuthentificationLogId)
                .ValueGeneratedOnAdd();

            builder.Property(al => al.DateCreation)
                .HasColumnType("datetime");
        }
    }
}

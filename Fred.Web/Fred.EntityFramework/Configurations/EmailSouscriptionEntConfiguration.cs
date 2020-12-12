using Fred.Entities.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class EmailSouscriptionEntConfiguration : IEntityTypeConfiguration<EmailSouscriptionEnt>
    {
        public void Configure(EntityTypeBuilder<EmailSouscriptionEnt> builder)
        {
            builder.ToTable("FRED_EMAIL_SUBSCRIPTION");
            builder.HasKey(es => es.EmailSouscriptionId);
            builder.Property(es => es.EmailSouscriptionId)
                .ValueGeneratedOnAdd();

            builder.Property(be => be.SouscriptionKey)
                .HasColumnName("EmailSouscriptionKey");
            builder.Property(cod => cod.DateDernierEnvoie)
                .HasColumnType("datetime");

            builder.HasOne(es => es.Personnel)
                .WithMany()
                .HasForeignKey(es => es.PersonnelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Fred.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class PersonnelImageEntConfiguration : IEntityTypeConfiguration<PersonnelImageEnt>
    {
        public void Configure(EntityTypeBuilder<PersonnelImageEnt> builder)
        {
            builder.ToTable("FRED_PERSONNEL_IMAGE");
            builder.HasKey(x => x.PersonnelImageId);

            builder.Property(x => x.PersonnelImageId)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Signature)
                .HasColumnType("image");
            builder.Property(x => x.PhotoProfil)
                .HasColumnType("image");
        }
    }
}

using Fred.Entities.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class ImageEntConfiguration : IEntityTypeConfiguration<ImageEnt>
    {
        public void Configure(EntityTypeBuilder<ImageEnt> builder)
        {
            builder.ToTable("FRED_IMAGE");
            builder.HasKey(i => i.ImageId);
            builder.Property(i => i.ImageId)
                .ValueGeneratedOnAdd();
        }
    }
}

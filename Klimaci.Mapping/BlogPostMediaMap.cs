using Klimaci.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Mapping
{
    public class BlogPostMediaMap : IEntityTypeConfiguration<BlogPostMedia>
    {
        public void Configure(EntityTypeBuilder<BlogPostMedia> b)
        {
            b.HasKey(x => new { x.BlogPostId, x.MediaId });
            b.Property(x => x.DisplayOrder).HasDefaultValue(0);

            b.HasOne(x => x.BlogPost).WithMany()
             .HasForeignKey(x => x.BlogPostId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Media).WithMany()
             .HasForeignKey(x => x.MediaId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

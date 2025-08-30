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
    public class BlogPostTagMap : IEntityTypeConfiguration<BlogPostTag>
    {
        public void Configure(EntityTypeBuilder<BlogPostTag> b)
        {
            b.HasKey(x => new { x.BlogPostId, x.TagId });

            b.HasOne(x => x.BlogPost).WithMany()
             .HasForeignKey(x => x.BlogPostId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Tag).WithMany()
             .HasForeignKey(x => x.TagId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

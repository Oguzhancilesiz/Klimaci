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
    public class BlogPostCategoryMap : IEntityTypeConfiguration<BlogPostCategory>
    {
        public void Configure(EntityTypeBuilder<BlogPostCategory> b)
        {
            b.HasKey(x => new { x.BlogPostId, x.CategoryId });

            b.HasOne(x => x.BlogPost).WithMany()
             .HasForeignKey(x => x.BlogPostId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Category).WithMany()
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

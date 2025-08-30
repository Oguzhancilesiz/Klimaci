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
    public class BlogPostMap : SlugEntityMap<BlogPost>, IEntityTypeConfiguration<BlogPost>
    {
        public new void Configure(EntityTypeBuilder<BlogPost> b)
        {
            base.Configure(b);
            b.Property(x => x.Summary).HasMaxLength(500);
        }
    }
}

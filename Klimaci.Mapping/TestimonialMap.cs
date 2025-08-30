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
    public class TestimonialMap : BaseMap<Testimonial>
    {
        public override void Configure(EntityTypeBuilder<Testimonial> b)
        {
            base.Configure(b);
            b.Property(x => x.Title).HasMaxLength(150).IsRequired();
            b.Property(x => x.Company).HasMaxLength(150);
            b.Property(x => x.Slug).HasMaxLength(220);
            b.Property(x => x.SeoTitle).HasMaxLength(150);
            b.Property(x => x.SeoDescription).HasMaxLength(300);
            b.HasIndex(x => x.Slug).IsUnique(false);
            b.HasIndex(x => x.DisplayOrder);
            b.HasIndex(x => x.IsPublished);
        }
    }
}

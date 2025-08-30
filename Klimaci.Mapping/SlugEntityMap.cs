using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klimaci.Entity;

namespace Klimaci.Mapping
{
    public class SlugEntityMap<T> : IEntityTypeConfiguration<T> where T : SlugEntity
    {
        public void Configure(EntityTypeBuilder<T> b)
        {
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.Slug).IsRequired().HasMaxLength(220);
            b.Property(x => x.SeoTitle).HasMaxLength(70);
            b.Property(x => x.SeoDescription).HasMaxLength(160);
            b.HasIndex(x => x.Slug)
             .IsUnique()
             .HasFilter("[Status] <> 4"); // 4 = Deleted
        }
    }
}

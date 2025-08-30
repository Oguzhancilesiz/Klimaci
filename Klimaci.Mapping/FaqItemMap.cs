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
    public class FaqItemMap : IEntityTypeConfiguration<FaqItem>
    {
        public void Configure(EntityTypeBuilder<FaqItem> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Question).IsRequired().HasMaxLength(300);
            b.Property(x => x.Answer).IsRequired();
            b.Property(x => x.DisplayOrder).HasDefaultValue(0);
            b.HasIndex(x => new { x.IsPublished, x.Status });
        }
    }
}

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
    public class PageMediaMap : IEntityTypeConfiguration<PageMedia>
    {
        public void Configure(EntityTypeBuilder<PageMedia> b)
        {
            b.HasKey(x => new { x.PageId, x.MediaId });
            b.Property(x => x.DisplayOrder).HasDefaultValue(0);

            b.HasOne(x => x.Page)
             .WithMany()
             .HasForeignKey(x => x.PageId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Media)
             .WithMany()
             .HasForeignKey(x => x.MediaId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

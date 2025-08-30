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
    public class ServiceMediaMap : IEntityTypeConfiguration<ServiceMedia>
    {
        public void Configure(EntityTypeBuilder<ServiceMedia> b)
        {
            b.HasKey(x => new { x.ServiceItemId, x.MediaId });
            b.Property(x => x.DisplayOrder).HasDefaultValue(0);

            b.HasOne(x => x.ServiceItem).WithMany()
             .HasForeignKey(x => x.ServiceItemId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Media).WithMany()
             .HasForeignKey(x => x.MediaId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

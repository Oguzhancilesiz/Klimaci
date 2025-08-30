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
    public class PartnerMediaMap : IEntityTypeConfiguration<PartnerMedia>
    {
        public void Configure(EntityTypeBuilder<PartnerMedia> b)
        {
            b.HasKey(x => new { x.PartnerId, x.MediaId });
            b.Property(x => x.DisplayOrder).HasDefaultValue(0);

            b.HasOne(x => x.Partner).WithMany()
             .HasForeignKey(x => x.PartnerId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Media).WithMany()
             .HasForeignKey(x => x.MediaId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Klimaci.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Mapping
{
    public class OfficeMap : BaseMap<Office>
    {
        public override void Configure(EntityTypeBuilder<Office> b)
        {
            base.Configure(b);
            b.ToTable("Offices");

            b.Property(x => x.Title).HasMaxLength(150).IsRequired();
            b.Property(x => x.Address).HasMaxLength(500);
            b.Property(x => x.City).HasMaxLength(100);
            b.Property(x => x.Phone).HasMaxLength(30);
            b.Property(x => x.Email).HasMaxLength(150);

            b.Property(x => x.Latitude).HasPrecision(9, 6);
            b.Property(x => x.Longitude).HasPrecision(9, 6);

            b.Property(x => x.DisplayOrder).HasDefaultValue(0);
            b.Property(x => x.IsPublished).HasDefaultValue(true);

            b.HasIndex(x => x.DisplayOrder);
            b.HasIndex(x => x.IsPublished);
            b.HasIndex(x => new { x.City, x.DisplayOrder });
        }
    }
}

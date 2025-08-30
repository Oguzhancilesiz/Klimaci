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
    public class ServiceItemMap : SlugEntityMap<ServiceItem>, IEntityTypeConfiguration<ServiceItem>
    {
        public new void Configure(EntityTypeBuilder<ServiceItem> b)
        {
            base.Configure(b);
            b.Property(x => x.ShortDescription).HasMaxLength(400);
        }
    }
}

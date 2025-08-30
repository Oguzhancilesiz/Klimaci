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
    public class PartnerMap : SlugEntityMap<Partner>, IEntityTypeConfiguration<Partner>
    {
        public new void Configure(EntityTypeBuilder<Partner> b)
        {
            base.Configure(b);
            b.Property(x => x.Website).HasMaxLength(300);
        }
    }
}

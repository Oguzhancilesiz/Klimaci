using Klimaci.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Mapping
{
    public class NavigationMenuMap : BaseMap<NavigationMenu>
    {
        public override void Configure(EntityTypeBuilder<NavigationMenu> b)
        {
            base.Configure(b);
            b.Property(x => x.Title).HasMaxLength(100).IsRequired();
            b.Property(x => x.Slug).HasMaxLength(50).IsRequired();
            b.HasIndex(x => x.Slug).IsUnique();
        }
    }
}

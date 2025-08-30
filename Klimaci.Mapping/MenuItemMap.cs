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
    public class MenuItemMap : BaseMap<MenuItem>
    {
        public override void Configure(EntityTypeBuilder<MenuItem> b)
        {
            base.Configure(b);
            b.Property(x => x.Title).HasMaxLength(120).IsRequired();
            b.Property(x => x.Url).HasMaxLength(500).IsRequired();
            b.Property(x => x.Target).HasMaxLength(20);
            b.HasIndex(x => new { x.NavigationMenuId, x.DisplayOrder });

            b.HasOne(x => x.NavigationMenu)
             .WithMany(m => m.Items)
             .HasForeignKey(x => x.NavigationMenuId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Parent)
             .WithMany(p => p.Children)
             .HasForeignKey(x => x.ParentId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

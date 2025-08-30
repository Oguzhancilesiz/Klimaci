using Klimaci.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Mapping
{
    public class SettingMap : BaseMap<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> b)
        {
            base.Configure(b);
            b.Property(x => x.Key).HasMaxLength(150).IsRequired();
            b.Property(x => x.Group).HasMaxLength(50);
            b.HasIndex(x => x.Key).IsUnique();
        }
    }
}

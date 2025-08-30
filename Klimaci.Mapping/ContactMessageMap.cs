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
    public class ContactMessageMap : BaseMap<ContactMessage>
    {
        public override void Configure(EntityTypeBuilder<ContactMessage> b)
        {
            base.Configure(b);
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.Property(x => x.Email).HasMaxLength(150).IsRequired();
            b.Property(x => x.Phone).HasMaxLength(30);
            b.Property(x => x.Subject).HasMaxLength(200);
            b.Property(x => x.IpAddress).HasMaxLength(45);
            b.HasIndex(x => x.CreatedDate);
            b.HasIndex(x => x.IsProcessed);
        }
    }
}

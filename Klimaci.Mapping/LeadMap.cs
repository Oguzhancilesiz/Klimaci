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
    public class LeadMap : BaseMap<Lead>
    {
        public override void Configure(EntityTypeBuilder<Lead> b)
        {
            base.Configure(b);

            b.ToTable("Leads");

            b.Property(x => x.Name).HasMaxLength(150).IsRequired();
            b.Property(x => x.Email).HasMaxLength(200);
            b.Property(x => x.Phone).HasMaxLength(30);

            b.Property(x => x.Source).HasMaxLength(50);
            b.Property(x => x.Subject).HasMaxLength(200);
            b.Property(x => x.Message).HasMaxLength(4000);

            b.Property(x => x.Notes).HasMaxLength(1000);

            b.HasIndex(x => x.IsProcessed);
            b.HasIndex(x => x.CreatedDate);
        }
    }
}

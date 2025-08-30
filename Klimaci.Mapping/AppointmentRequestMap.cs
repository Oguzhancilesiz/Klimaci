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
    public class AppointmentRequestMap : BaseMap<AppointmentRequest>
    {
        public override void Configure(EntityTypeBuilder<AppointmentRequest> b)
        {
            base.Configure(b);
            b.ToTable("AppointmentRequests");

            b.Property(x => x.Name).HasMaxLength(150).IsRequired();
            b.Property(x => x.Email).HasMaxLength(200);
            b.Property(x => x.Phone).HasMaxLength(30);

            b.Property(x => x.PreferredTime).HasMaxLength(50);
            b.Property(x => x.Notes).HasMaxLength(2000);

            b.HasIndex(x => x.IsProcessed);
            b.HasIndex(x => x.CreatedDate);
        }
    }
}

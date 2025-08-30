using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klimaci.Entity;

namespace Klimaci.Mapping
{
    public class MediaMap : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.FileUrl).IsRequired().HasMaxLength(600);
            b.Property(x => x.AltText).HasMaxLength(200);
            b.Property(x => x.Title).HasMaxLength(200);
            b.HasIndex(x => x.Status);
        }
    }
}

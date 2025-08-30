using Klimaci.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Mapping
{
    public class UrlRedirectMap : BaseMap<UrlRedirect>
    {
        public override void Configure(EntityTypeBuilder<UrlRedirect> b)
        {
            base.Configure(b);
            b.Property(x => x.OldPath).HasMaxLength(300).IsRequired();
            b.Property(x => x.NewPath).HasMaxLength(300).IsRequired();
            b.HasIndex(x => x.OldPath).IsUnique();
            b.HasIndex(x => x.IsPublished);
        }
    }
}

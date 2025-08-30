using Klimaci.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Mapping
{
    public class TeamMemberMap : BaseMap<TeamMember>
    {
        public override void Configure(EntityTypeBuilder<TeamMember> b)
        {
            base.Configure(b);
            b.Property(x => x.Name).HasMaxLength(120).IsRequired();
            b.Property(x => x.TitleText).HasMaxLength(120);
            b.Property(x => x.Email).HasMaxLength(150);
            b.Property(x => x.Phone).HasMaxLength(30);
            b.Property(x => x.Slug).HasMaxLength(220);
            b.Property(x => x.SeoTitle).HasMaxLength(150);
            b.Property(x => x.SeoDescription).HasMaxLength(300);
            b.HasIndex(x => x.Slug);
            b.HasIndex(x => x.DisplayOrder);
            b.HasIndex(x => x.IsPublished);
        }
    }
}

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
    public class ProjectMap : SlugEntityMap<Project>, IEntityTypeConfiguration<Project>
    {
        public new void Configure(EntityTypeBuilder<Project> b)
        {
            base.Configure(b);
            b.Property(x => x.City).HasMaxLength(120);
        }
    }
}

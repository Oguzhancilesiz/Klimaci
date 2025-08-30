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
    public class TagMap : SlugEntityMap<Tag>, IEntityTypeConfiguration<Tag>
    {
        public new void Configure(EntityTypeBuilder<Tag> b)
        {
            base.Configure(b);
        }
    }
}

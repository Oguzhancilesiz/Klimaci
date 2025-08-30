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
    public class PageMap : SlugEntityMap<Page>, IEntityTypeConfiguration<Page>
    {
        public new void Configure(EntityTypeBuilder<Page> b)
        {
            base.Configure(b);
            // Content nvarchar(max) default
        }
    }
}

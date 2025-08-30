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
    public class ProjectMediaMap : IEntityTypeConfiguration<ProjectMedia>
    {
        public void Configure(EntityTypeBuilder<ProjectMedia> b)
        {
            b.HasKey(x => new { x.ProjectId, x.MediaId });
            b.Property(x => x.DisplayOrder).HasDefaultValue(0);

            b.HasOne(x => x.Project).WithMany()
             .HasForeignKey(x => x.ProjectId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Media).WithMany()
             .HasForeignKey(x => x.MediaId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

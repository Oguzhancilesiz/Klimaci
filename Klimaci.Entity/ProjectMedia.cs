using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class ProjectMedia : MediaLinkBase
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}

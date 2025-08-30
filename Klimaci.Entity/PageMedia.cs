using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class PageMedia : MediaLinkBase
    {
        public Guid PageId { get; set; }
        public Page Page { get; set; } = null!;
    }
}

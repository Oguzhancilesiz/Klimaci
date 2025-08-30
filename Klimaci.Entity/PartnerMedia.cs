using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class PartnerMedia : MediaLinkBase
    {
        public Guid PartnerId { get; set; }
        public Partner Partner { get; set; } = null!;
    }
}

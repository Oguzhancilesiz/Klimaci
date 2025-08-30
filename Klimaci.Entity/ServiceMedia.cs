using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class ServiceMedia : MediaLinkBase
    {
        public Guid ServiceItemId { get; set; }
        public ServiceItem ServiceItem { get; set; } = null!;
    }
}

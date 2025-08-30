using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class BlogPostMedia : MediaLinkBase
    {
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = null!;
    }
}

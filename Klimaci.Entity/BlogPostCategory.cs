using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class BlogPostCategory
    {
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}

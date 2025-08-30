using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class BlogPost : SlugEntity
    {
        public string? Summary { get; set; }
        public string Content { get; set; } = "";
        public DateTime? PublishDate { get; set; }
    }
}

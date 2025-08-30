using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public abstract class SlugEntity : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsPublished { get; set; } = true;
    }
}

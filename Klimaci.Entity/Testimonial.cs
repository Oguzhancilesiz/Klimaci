using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class Testimonial : BaseEntity
    {
        public string FullName { get; set; }
        public string Title { get; set; } = "";          // kişi adı veya başlık
        public string? Company { get; set; }
        public string Content { get; set; } = "";
        public int? Rating { get; set; }                 // 1-5
        public string? Slug { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsPublished { get; set; } = true;

        public Guid? MediaId { get; set; }               // avatar/foto (opsiyonel)
    }
}

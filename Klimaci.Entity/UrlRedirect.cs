using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class UrlRedirect : BaseEntity
    {
        public string OldPath { get; set; } = "";  // "/eski-slug"
        public string NewPath { get; set; } = "";  // "/yeni-slug"
        public int StatusCode { get; set; } = 301; // 301/302
        public bool IsPublished { get; set; } = true;
    }
}

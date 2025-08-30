using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class FaqItem : BaseEntity
    {
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public int DisplayOrder { get; set; } = 0;
        public bool IsPublished { get; set; } = true;
    }
}

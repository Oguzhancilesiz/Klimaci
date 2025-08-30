using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class Partner : SlugEntity
    {
        public string? Website { get; set; }
        public bool IsFeatured { get; set; } = true;
    }
}

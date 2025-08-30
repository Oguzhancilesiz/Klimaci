using Klimaci.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class Lead : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? Source { get; set; }   // "contact", "landing", "ads" vs.
        public string? Subject { get; set; }
        public string? Message { get; set; }

        public bool IsProcessed { get; set; }
        public string? Notes { get; set; }
    }
}

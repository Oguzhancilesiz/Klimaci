using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class ContactMessage : BaseEntity
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string? Subject { get; set; }
        public string Message { get; set; } = "";
        public string? IpAddress { get; set; }
        public bool IsProcessed { get; set; } = false;
        public DateTime? ProcessedAt { get; set; }
        public string? Notes { get; set; }  // admin iç not
    }
}

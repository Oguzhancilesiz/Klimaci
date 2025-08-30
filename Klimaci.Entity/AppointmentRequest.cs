using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class AppointmentRequest : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public DateTime? PreferredDate { get; set; }
        public string? PreferredTime { get; set; }   // "09:00-12:00" gibi basit string yeter
        public string? Notes { get; set; }

        public bool IsProcessed { get; set; }
    }
}

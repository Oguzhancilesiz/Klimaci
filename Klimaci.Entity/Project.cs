using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class Project : SlugEntity
    {
        public string? Description { get; set; }
        public string? City { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid? BrandId { get; set; } // opsiyonel
        public Brand? Brand { get; set; }
    }
}

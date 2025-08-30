using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class SiteSetting : BaseEntity
    {
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? MapEmbedUrl { get; set; }
        public string? WorkingHours { get; set; }
        public string? HeroTitle { get; set; }
        public string? HeroSubtitle { get; set; }
    }
}

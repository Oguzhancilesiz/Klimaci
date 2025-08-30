using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public abstract class MediaLinkBase
    {
        public Guid MediaId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsCover { get; set; } = false;
        public string? OverrideAltText { get; set; }
        public string? OverrideTitle { get; set; }
        public Media Media { get; set; } = null!;
    }
}

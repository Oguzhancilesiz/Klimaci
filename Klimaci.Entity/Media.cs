using Klimaci.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class Media : BaseEntity
    {
        public string FileUrl { get; set; } = null!;
        public string? AltText { get; set; }
        public string? Title { get; set; }
        public MediaType MediaType { get; set; } = MediaType.Image;
        public long? FileSizeBytes { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? DurationSec { get; set; }
    }
}

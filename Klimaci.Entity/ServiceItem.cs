using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class ServiceItem : SlugEntity
    {
        public string? ShortDescription { get; set; }
        public string Content { get; set; } = "";
        public decimal? PriceFrom { get; set; } // istersen
    }
}

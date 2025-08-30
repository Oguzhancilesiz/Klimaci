using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; } = "";          // "Site:Title", "Site:Phone", "Seo:DefaultTitle" ...
        public string? Value { get; set; }             // JSON/string
        public string? Group { get; set; }             // "Site", "Seo", "Contact"
        public string? Description { get; set; }
    }
}

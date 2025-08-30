using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class MenuItem : BaseEntity
    {
        public Guid NavigationMenuId { get; set; }
        public string Title { get; set; } = "";
        public string Url { get; set; } = "#";
        public int DisplayOrder { get; set; } = 0;
        public bool IsPublished { get; set; } = true;
        public string? Target { get; set; }            // _self / _blank
        public Guid? ParentId { get; set; }

        public NavigationMenu NavigationMenu { get; set; } = null!;
        public MenuItem? Parent { get; set; }
        public ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}

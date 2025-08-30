using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class NavigationMenu : BaseEntity
    {
        public string Title { get; set; } = "Main";
        public string Slug { get; set; } = "main";     // ör: main, footer
        public bool IsPublished { get; set; } = true;

        public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
    }
}

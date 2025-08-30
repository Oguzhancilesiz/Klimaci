using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class TeamMember : BaseEntity
    {
        public string Name { get; set; } = "";
        public string? TitleText { get; set; }         // Ünvan
        public string? Bio { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? Slug { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsPublished { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.NavigationDTOs
{
    public record NavigationMenuUpdateDTO(Guid Id, string Title, string Slug, bool IsPublished);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.NavigationDTOs
{
    public record MenuItemAddDTO(Guid NavigationMenuId, string Title, string Url, string? Target,
                                  Guid? ParentId, int DisplayOrder, bool IsPublished);
}

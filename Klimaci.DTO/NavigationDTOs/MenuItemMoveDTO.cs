using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.NavigationDTOs
{
    public record MenuItemMoveDTO(Guid Id, Guid? NewParentId);
}

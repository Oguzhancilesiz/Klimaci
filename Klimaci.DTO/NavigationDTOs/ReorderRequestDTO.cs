using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.NavigationDTOs
{
    public record ReorderRequestDTO(IEnumerable<ReorderItemDTO> Items);

}

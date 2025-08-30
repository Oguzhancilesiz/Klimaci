using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ContactMessageDTOs
{
    public record ContactProcessDTO(Guid Id, bool IsProcessed, string? Notes);

}

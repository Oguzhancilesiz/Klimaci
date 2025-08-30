using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ContactDTOs
{
    public record ContactMessageUpdateDTO(
     Guid Id,
     bool IsRead
 );
}

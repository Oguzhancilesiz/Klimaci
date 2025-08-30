using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.MediaDTOs
{
    public record MediaUpdateDTO(
         Guid Id,
         string? AltText = null,
         string? Title = null
     );
}

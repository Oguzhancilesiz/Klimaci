using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO
{
    public record GalleryUpdateDTO(
         Guid OwnerId,
         List<Guid> MediaIds,
         Guid? CoverMediaId
     );
}

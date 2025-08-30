using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO
{
    public record GalleryItemDTO(
       Guid MediaId,
       string FileUrl,
       bool IsCover,
       int DisplayOrder,
       string? AltText = null,
       string? Title = null
   );
}

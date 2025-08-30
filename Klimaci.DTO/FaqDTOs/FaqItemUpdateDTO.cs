using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.FaqDTOs
{
    public record FaqItemUpdateDTO(
         Guid Id,
         string Question,
         string Answer,
         int DisplayOrder = 0,
         bool IsPublished = true
     );
}

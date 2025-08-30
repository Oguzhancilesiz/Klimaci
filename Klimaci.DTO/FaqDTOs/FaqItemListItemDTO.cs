using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.FaqDTOs
{
    public record FaqItemListItemDTO(Guid Id, string Question, bool IsPublished, int DisplayOrder);
}

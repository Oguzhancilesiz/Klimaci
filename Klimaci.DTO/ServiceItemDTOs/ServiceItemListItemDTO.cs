using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ServiceItemDTOs
{
    public record ServiceItemListItemDTO(Guid Id, string Title, string Slug, bool IsPublished, int DisplayOrder, string? CoverUrl);
}

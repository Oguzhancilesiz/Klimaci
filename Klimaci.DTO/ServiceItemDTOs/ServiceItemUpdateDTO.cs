using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ServiceItemDTOs
{
    public record ServiceItemUpdateDTO(
         Guid Id,
         string Title,
         string? ShortDescription,
         string Content,
         decimal? PriceFrom,
         string? Slug = null,
         string? SeoTitle = null,
         string? SeoDescription = null,
         int DisplayOrder = 0,
         bool IsPublished = true,
         List<Guid>? MediaIds = null,
         Guid? CoverMediaId = null
     );
}

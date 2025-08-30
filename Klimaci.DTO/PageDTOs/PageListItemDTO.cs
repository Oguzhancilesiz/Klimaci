using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.PageDTOs
{
    public record PageListItemDTO(Guid Id, string Title, string Slug, bool IsPublished, DateTime CreatedDate, string? CoverUrl);
}

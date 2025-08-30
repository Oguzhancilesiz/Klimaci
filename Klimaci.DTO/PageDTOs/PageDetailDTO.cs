using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.PageDTOs
{
    public record PageDetailDTO(Guid Id, string Title, string Slug, string Content, bool IsPublished,
                                string? SeoTitle, string? SeoDescription, int DisplayOrder,
                                List<Guid> MediaIds, Guid? CoverMediaId);
}

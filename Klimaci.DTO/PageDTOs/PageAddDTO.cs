using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.PageDTOs
{
    public record PageAddDTO(
        string Title,
        string Content,
        string? Slug = null,
        string? SeoTitle = null,
        string? SeoDescription = null,
        int DisplayOrder = 0,
        bool IsPublished = true,
        List<Guid>? MediaIds = null,
        Guid? CoverMediaId = null
    );
}

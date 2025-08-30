using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.BlogDTOs
{
    public record CategoryAddDTO(
        string Title,
        string? Slug = null,
        string? SeoTitle = null,
        string? SeoDescription = null,
        int DisplayOrder = 0,
        bool IsPublished = true
    );
}

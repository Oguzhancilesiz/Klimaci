using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.BlogDTOs
{
    public record BlogPostListItemDTO(Guid Id, string Title, string Slug, DateTime CreatedDate, bool IsPublished, string? CoverUrl);
}

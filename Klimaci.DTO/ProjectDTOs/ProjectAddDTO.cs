using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ProjectDTOs
{
    public record ProjectAddDTO(
       string Title,
       string? Description,
       string? City,
       DateTime? CompletedAt,
       Guid? BrandId,
       string? Slug = null,
       string? SeoTitle = null,
       string? SeoDescription = null,
       int DisplayOrder = 0,
       bool IsPublished = true,
       List<Guid>? MediaIds = null,
       Guid? CoverMediaId = null
   );
}

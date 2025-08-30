using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ProjectDTOs
{
    public record BrandUpdateDTO(
           Guid Id,
           string Title,
           string? Website,
           string? Slug = null,
           string? SeoTitle = null,
           string? SeoDescription = null,
           int DisplayOrder = 0,
           bool IsPublished = true
       );
}

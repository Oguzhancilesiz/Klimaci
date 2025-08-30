using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.PartnerDTOs
{
    public record PartnerUpdateDTO(
      Guid Id,
      string Title,
      string? Website,
      bool IsFeatured = true,
      string? Slug = null,
      string? SeoTitle = null,
      string? SeoDescription = null,
      int DisplayOrder = 0,
      bool IsPublished = true,
      List<Guid>? MediaIds = null,
      Guid? CoverMediaId = null
  );
}

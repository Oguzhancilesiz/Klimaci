using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.PartnerDTOs
{
    public record PartnerListItemDTO(Guid Id, string Title, string Slug, bool IsFeatured, string? CoverUrl);
}


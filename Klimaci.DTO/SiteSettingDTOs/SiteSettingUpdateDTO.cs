using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.SiteSettingDTOs
{
    public record SiteSettingUpdateDTO(
       Guid Id,
       string Phone,
       string Email,
       string Address,
       string? MapEmbedUrl,
       string? WorkingHours,
       string? HeroTitle,
       string? HeroSubtitle
   );
}

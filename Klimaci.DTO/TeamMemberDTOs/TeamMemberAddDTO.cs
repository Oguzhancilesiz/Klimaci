using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.TeamMemberDTOs
{
    public record TeamMemberAddDTO(
         string Name, string? TitleText, string? Bio, string? Email, string? Phone,
         string? Slug, string? SeoTitle, string? SeoDescription,
         int DisplayOrder, bool IsPublished);

}

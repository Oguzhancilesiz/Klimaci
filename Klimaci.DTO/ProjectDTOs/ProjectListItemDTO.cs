using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ProjectDTOs
{
    public record ProjectListItemDTO(Guid Id, string Title, string Slug, string? City, DateTime CreatedDate, string? CoverUrl, bool IsPublished);
}

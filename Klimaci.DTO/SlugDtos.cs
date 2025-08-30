using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO
{
    // Admin panelde "slug uygun mu?" kontrolü için hafif endpoint
    public record SlugCheckDTO(string Title, string? Slug, Guid? ExcludeId);
    public record SlugCheckResultDTO(bool IsUnique, string FinalSlug);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.UrlRedirectDTOs
{
    public record UrlRedirectAddDTO(string OldPath, string NewPath, int StatusCode, bool IsPublished);

}

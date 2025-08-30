using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO
{
    public record IdNameDTO(Guid Id, string Name);
    public record IdTitleDTO(Guid Id, string Title);
}

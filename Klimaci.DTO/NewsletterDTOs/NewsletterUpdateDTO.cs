using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.NewsletterDTOs
{
    public record NewsletterUpdateDTO(Guid Id, string Email, string? Name);

}

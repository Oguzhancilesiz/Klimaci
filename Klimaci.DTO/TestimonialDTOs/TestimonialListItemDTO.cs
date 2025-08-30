using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.TestimonialDTOs
{
    public record TestimonialListItemDTO(Guid Id, string FullName, string? Company, int Rating, bool IsApproved, DateTime CreatedDate);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.TestimonialDTOs
{
    public record TestimonialUpdateDTO(
       Guid Id,
       string FullName,
       string Content,
       string? Company,
       int? Rating = 5,
       bool IsApproved = false,
       string? Slug = "",
       string? Title = "",
      string SeoTitle = "",
      string SeoDescription = "",
        int DisplayOrder = 0,
        bool IsPublished = true,
        Guid? MediaId = null
   );

}

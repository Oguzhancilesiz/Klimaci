using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ContactDTOs
{
    public record ContactMessageAddDTO(
      string FullName,
      string Email,
      string? Phone,
      string Message
  );
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.ContactMessageDTOs
{
    public record ContactMessageAddDTO(string Name, string Email, string? Phone, string? Subject, string Message);
}

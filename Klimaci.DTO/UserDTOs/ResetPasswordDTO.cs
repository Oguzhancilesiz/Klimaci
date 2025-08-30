using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.UserDTOs
{
    public record ResetPasswordDTO
    {
        [Required] public Guid Id { get; init; }
        [Required, MinLength(6)] public string NewPassword { get; init; } = "";
    }
}

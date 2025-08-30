using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO.UserDTOs
{
    public record UserUpdateDTO
    {
        [Required] public Guid Id { get; init; }
        [Required, MinLength(3)] public string UserName { get; init; } = "";
        [Required, EmailAddress] public string Email { get; init; } = "";
        public bool IsActive { get; init; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.DTO
{
    public record AppointmentRequestAddDTO(
     string FullName,
     string Phone,
     string? Email,
     string? Address,
     string? Note,
     DateTime? PreferredDate
 );

    public record AppointmentRequestUpdateDTO(
        Guid Id,
        string FullName,
        string Phone,
        string? Email,
        string? Address,
        string? Note,
        DateTime? PreferredDate,
        bool IsHandled
    );

    public record AppointmentRequestListItemDTO(
        Guid Id,
        string FullName,
        string Phone,
        string? Email,
        bool IsHandled,
        DateTime CreatedDate
    );
}

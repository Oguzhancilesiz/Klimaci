using Klimaci.Core.Paging;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IAppointmentRequestService
    {
        Task<PagedResult<AppointmentRequest>> PagedAsync(int page, int pageSize, string? search, bool onlyUnprocessed);
        Task<AppointmentRequest?> GetByIdAsync(Guid id);
        Task ProcessAsync(Guid id, bool isProcessed, string? notes);
        Task DeleteAsync(Guid id);
    }
}

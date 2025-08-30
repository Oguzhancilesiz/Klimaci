using Klimaci.Core.Paging;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface ILeadService
    {
        Task<PagedResult<Lead>> PagedAsync(int page, int pageSize, string? search, bool onlyUnprocessed);
        Task<Lead?> GetByIdAsync(Guid id);
        Task ProcessAsync(Guid id, bool isProcessed, string? notes);
        Task DeleteAsync(Guid id);

        // İstersen kamusal formdan eklemek için:
        Task AddAsync(string name, string? email, string? phone, string? source, string? subject, string? message);
    }
}

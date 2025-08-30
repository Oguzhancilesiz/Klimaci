using Klimaci.Core.Paging;
using Klimaci.DTO.ContactMessageDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IContactMessageService
    {
        Task AddAsync(ContactMessageAddDTO dto, string? ipAddress);
        Task<PagedResult<ContactMessage>> PagedAsync(int page = 1, int pageSize = 20, string? search = null, bool onlyUnprocessed = false);
        Task<ContactMessage?> GetByIdAsync(Guid id);
        Task ProcessAsync(ContactProcessDTO dto);
        Task DeleteAsync(Guid id);
    }
}

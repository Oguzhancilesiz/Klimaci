using Klimaci.Core.Paging;
using Klimaci.DTO.UrlRedirectDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface IUrlRedirectService
    {
        Task<PagedResult<UrlRedirect>> PagedAsync(int page = 1, int pageSize = 30, string? search = null);
        Task<UrlRedirect?> GetByIdAsync(Guid id);
        Task AddAsync(UrlRedirectAddDTO dto);
        Task UpdateAsync(UrlRedirectUpdateDTO dto);
        Task DeleteAsync(Guid id);
    }
}

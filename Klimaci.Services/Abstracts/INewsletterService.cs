using Klimaci.Core.Paging;
using Klimaci.DTO.NewsletterDTOs;
using Klimaci.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Abstracts
{
    public interface INewsletterService
    {
        Task<(bool ok, string? error)> SubscribeAsync(NewsletterSubscribeDTO dto);
        Task<(bool ok, string? error)> ConfirmAsync(Guid id, string token);
        Task<(bool ok, string? error)> UnsubscribeAsync(Guid id);
        Task<PagedResult<NewsletterSubscriber>> PagedAsync(
            int page = 1,
            int pageSize = 40,
            string? search = null,
            bool onlyActive = false
        );
        Task<NewsletterSubscriber?> GetByIdAsync(Guid id);
        Task UpdateAsync(NewsletterUpdateDTO dto);
        Task DeleteAsync(Guid id);
        Task<(bool ok, string? error)> AdminConfirmAsync(Guid id);
    }
}

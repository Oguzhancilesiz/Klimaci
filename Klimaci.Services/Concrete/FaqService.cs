using Klimaci.Core.Abstracts;
using Klimaci.Core.Paging;
using Klimaci.DTO;
using Klimaci.DTO.BlogDTOs;
using Klimaci.DTO.FaqDTOs;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Services.Concrete
{
    public class FaqService : IFaqService
    {
        private readonly IUnitOfWork _uow;
        public FaqService(IUnitOfWork uow) { _uow = uow; }

        public async Task Add(FaqItemAddDTO dto)
        {
            var repo = _uow.Repository<FaqItem>(); var e = dto.Adapt<FaqItem>(); e.Id = Guid.NewGuid();
            await repo.AddAsync(e); await _uow.SaveChangesAsync();
        }

        public async Task Update(FaqItemUpdateDTO dto)
        {
            var repo = _uow.Repository<FaqItem>(); var e = await repo.GetById(dto.Id); if (e is null) return;
            dto.Adapt(e); await repo.Update(e); await _uow.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var repo = _uow.Repository<FaqItem>(); var e = await repo.GetById(id); if (e is null) return;
            await repo.Delete(e); await _uow.SaveChangesAsync();
        }

        public Task<List<FaqItem>> GetAll() => _uow.Repository<FaqItem>().GetAll();
        public Task<FaqItem> GetById(Guid id) => _uow.Repository<FaqItem>().GetById(id);

        public async Task<PagedResult<FaqItem>> PagedAsync(int page = 1, int pageSize = 30, string? search = null)
        {
            Expression<Func<FaqItem, bool>> pred = x => string.IsNullOrWhiteSpace(search) || x.Question.Contains(search);
            var (items, total) = await _uow.Repository<FaqItem>().PagedAsync(page, pageSize, pred, iq => iq.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Question));
            return new(items, total, page, pageSize);
        }

        public async Task ReorderAsync(IEnumerable<ReorderItemDTO> items)
        {
            var repo = _uow.Repository<FaqItem>();
            foreach (var it in items ?? Enumerable.Empty<ReorderItemDTO>())
            {
                var e = await repo.GetById(it.Id); if (e is null) continue;
                e.DisplayOrder = it.DisplayOrder; await repo.Update(e);
            }
            await _uow.SaveChangesAsync();
        }
    }
}

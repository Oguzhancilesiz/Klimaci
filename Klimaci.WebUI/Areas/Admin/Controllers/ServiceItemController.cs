using Klimaci.Core.Abstracts;
using Klimaci.DTO.ServiceItemDTOs;
using Klimaci.DTO;
using Klimaci.Entity;
using Klimaci.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class ServiceItemController : Controller
    {
        private readonly IServiceItemService _svc;
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _env;
        public ServiceItemController(IServiceItemService svc, IUnitOfWork uow, IWebHostEnvironment env) { _svc = svc; _uow = uow; _env = env; }

        [HttpGet] public async Task<IActionResult> Index([FromQuery] ListQueryDTO q) => View(await _svc.PagedAsync(q ?? new()));

        [HttpGet] public IActionResult Create() => View(new ServiceItemAddDTO("", null, "", null, null, null, null, 0, true, null, null));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceItemAddDTO dto, List<IFormFile> files)
        {
            var uploaded = await SaveMediasAsync(files, "uploads/media");
            dto = dto with { MediaIds = (dto.MediaIds ?? new()).Concat(uploaded).Distinct().ToList() };
            await _svc.Add(dto); TempData["ok"] = "Hizmet eklendi."; return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var e = await _svc.GetById(id); if (e == null) return NotFound();
            ViewBag.Gallery = await _svc.GetGalleryAsync(id);
            var dto = new ServiceItemUpdateDTO(e.Id, e.Title, e.ShortDescription, e.Content, e.PriceFrom, e.Slug, e.SeoTitle, e.SeoDescription, e.DisplayOrder, e.IsPublished, null, null);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceItemUpdateDTO dto, List<IFormFile> newFiles)
        {
            var uploaded = await SaveMediasAsync(newFiles, "uploads/media");
            dto = dto with { MediaIds = (dto.MediaIds ?? new()).Concat(uploaded).Distinct().ToList() };
            await _svc.Update(dto); TempData["ok"] = "Hizmet güncellendi."; return RedirectToAction(nameof(Index));
        }

        [HttpPost][ValidateAntiForgeryToken] public async Task<IActionResult> Delete([FromForm] IdDTO dto) { await _svc.Delete(dto.Id); TempData["ok"] = "Silindi."; return RedirectToAction(nameof(Index)); }
        [HttpPost] public async Task<IActionResult> TogglePublish([FromBody] TogglePublishDTO dto) { await _svc.TogglePublishAsync(dto.Id, dto.IsPublished); return Ok(); }
        [HttpPost] public async Task<IActionResult> Reorder([FromBody] ReorderRequestDTO dto) { await _svc.ReorderAsync(dto.Items ?? new()); return Ok(); }
        [HttpPost] public async Task<IActionResult> SlugCheck([FromBody] SlugCheckDTO dto) { return Ok(await _svc.SlugCheckAsync(dto)); }
        [HttpPost] public async Task<IActionResult> GalleryUpdate([FromBody] GalleryUpdateDTO dto) { await _svc.GalleryUpdateAsync(dto); return Ok(); }

        private async Task<List<Guid>> SaveMediasAsync(List<IFormFile> files, string relFolder)
        {
            var ids = new List<Guid>(); if (files == null || files.Count == 0) return ids;
            var root = Path.Combine(_env.WebRootPath, relFolder.Replace("/", Path.DirectorySeparatorChar.ToString())); Directory.CreateDirectory(root);
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            foreach (var f in files.Where(f => f?.Length > 0))
            {
                var ext = Path.GetExtension(f.FileName).ToLowerInvariant(); if (!allowed.Contains(ext)) continue;
                var name = $"{Guid.NewGuid():N}{ext}"; var full = Path.Combine(root, name); using var fs = System.IO.File.Create(full); await f.CopyToAsync(fs);
                var media = new Media { Id = Guid.NewGuid(), FileUrl = "/" + $"{relFolder}/{name}".Replace("\\", "/"), Title = Path.GetFileNameWithoutExtension(f.FileName) };
                await _uow.Repository<Media>().AddAsync(media); ids.Add(media.Id);
            }
            await _uow.SaveChangesAsync(); return ids;
        }
    }
}

using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEd.Controllers
{
    public class PluginController : Controller
    {
        private readonly IPluginRepository _repository;

        public PluginController(IPluginRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var plugins = await _repository.GetAllAsync();

            var viewModels = plugins.Select(p => new PluginViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                //DownloadLink = p.DownloadLink,
                //ImageUrl = p.ImageUrl,
                
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = Enum.GetValues(typeof(Category))
              .Cast<Category>()
              .Select(e => new SelectListItem
              {
                  Text = e.ToString(),
                  Value = e.ToString()
              }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PluginViewModel viewModel,IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }

                    //viewModel.ImageUrl = fileName;
                }

                var plugin = new Plugin
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    //DownloadLink = viewModel.DownloadLink,
                    //ImageUrl = viewModel.ImageUrl
                    
                };

                await _repository.InsertAsync(plugin);
                await _repository.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = Enum.GetValues(typeof(Category))
              .Cast<Category>()
              .Select(e => new SelectListItem
              {
                  Text = e.ToString(),
                  Value = e.ToString()
              }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var plugin = await _repository.GetByIdAsync(id);
            if (plugin == null)
                return NotFound();

            var viewModel = new PluginViewModel
            {
                Id = plugin.Id,
                Title = plugin.Title,
                Description = plugin.Description,
                Price = plugin.Price,
                //DownloadLink = plugin.DownloadLink,
                CreatedAt = plugin.CreatedAt,
                UpdatedAt = plugin.UpdatedAt,
                //ImageUrl = plugin.ImageUrl
                
            };

            ViewBag.Categories = Enum.GetValues(typeof(Category))
              .Cast<Category>()
              .Select(e => new SelectListItem
              {
                  Text = e.ToString(),
                  Value = e.ToString()
              }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PluginViewModel viewModel, IFormFile ImageUrl, IFormFile DownloadLink)
        {
            if (id != viewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var plugin = await _repository.GetByIdAsync(id);
                if (plugin == null)
                    return NotFound();

                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(stream);
                    }

                    //plugin.ImageUrl = fileName;
                }

                if (DownloadLink != null && DownloadLink.Length > 0)
                {
                    var downloadFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(DownloadLink.FileName);
                    var downloadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", downloadFileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadPath)!);

                    using (var stream = new FileStream(downloadPath, FileMode.Create))
                    {
                        await DownloadLink.CopyToAsync(stream);
                    }

                    //plugin.DownloadLink = downloadFileName;
                }

                plugin.Title = viewModel.Title;
                plugin.Description = viewModel.Description;
                plugin.Price = viewModel.Price;
               
                plugin.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(plugin);
                await _repository.SaveAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = e.ToString()
                }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var plugin = await _repository.GetByIdAsync(id);
            if (plugin == null)
                return NotFound();

            var viewModel = new PluginViewModel
            {
                Id = plugin.Id,
                Title = plugin.Title,
                Description = plugin.Description,
                Price = plugin.Price,
                CreatedAt = plugin.CreatedAt,
                UpdatedAt = plugin.UpdatedAt,
                //DownloadLink = plugin.DownloadLink,
                //ImageUrl = plugin.ImageUrl,
               
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

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
        private readonly IMapper _mapper;

        public PluginController(IPluginRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var plugins = await _repository.GetAllAsync();
            var viewModels = _mapper.Map<List<PluginViewModel>>(plugins);
            return View(viewModels);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PluginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var plugin = _mapper.Map<Plugin>(viewModel);
                plugin.CreatedAt = DateTime.UtcNow; // Manually set timestamps
                plugin.UpdatedAt = DateTime.UtcNow;


             ViewBag.Categories = new SelectList(Enum.GetValues(typeof(Category))
            .Cast<Category>()
            .Select(e => new SelectListItem
            {
                Text = e.ToString().Replace("_", " "), // Directly format here
                Value = e.ToString()
            }));
                await _repository.InsertAsync(plugin);
                await _repository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var plugin = await _repository.GetByIdAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<PluginViewModel>(plugin);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PluginViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingPlugin = await _repository.GetByIdAsync(id);
                if (existingPlugin == null)
                {
                    return NotFound();
                }

                // Map ViewModel → Entity (ignoring CreatedAt/UpdatedAt)
                _mapper.Map(viewModel, existingPlugin);
                existingPlugin.UpdatedAt = DateTime.UtcNow; // Update timestamp

                _repository.UpdateAsync(existingPlugin);
                await _repository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var plugin = await _repository.GetByIdAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<PluginViewModel>(plugin);
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

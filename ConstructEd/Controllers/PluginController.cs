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
        private readonly IPluginRepository _pluginRepository;
        private readonly IMapper _mapper;

        public PluginController(IPluginRepository repository, IMapper mapper)
        {
            _pluginRepository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var plugins = await _pluginRepository.GetAllAsync();
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
                await _pluginRepository.InsertAsync(plugin);
                await _pluginRepository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var plugin = await _pluginRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<PluginViewModel>(plugin);
            return View(nameof(Details), plugin);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var plugin = await _pluginRepository.GetByIdAsync(id);
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
                var existingPlugin = await _pluginRepository.GetByIdAsync(id);
                if (existingPlugin == null)
                {
                    return NotFound();
                }

                // Map ViewModel → Entity (ignoring CreatedAt/UpdatedAt)
                _mapper.Map(viewModel, existingPlugin);
                existingPlugin.UpdatedAt = DateTime.UtcNow; // Update timestamp

                _pluginRepository.UpdateAsync(existingPlugin);
                await _pluginRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var plugin = await _pluginRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<PluginViewModel>(plugin);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pluginRepository.DeleteAsync(id);
            await _pluginRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

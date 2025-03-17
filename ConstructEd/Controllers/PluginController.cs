using System.Security.Claims;
using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class PluginController : Controller
    {
        private readonly IPluginRepository _pluginRepository;
        private readonly IMapper _mapper;
        private readonly IWishListRepository _wishlistRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public PluginController(IPluginRepository pluginRepository, IMapper mapper,
                                IWishListRepository wishlistRepository,
                                IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _wishlistRepository = wishlistRepository;
            _pluginRepository = pluginRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var plugins = await _pluginRepository.GetAllAsync();
            var pluginViewModels = _mapper.Map<List<PluginViewModel>>(plugins);

            if (!string.IsNullOrEmpty(userId))
            {
                foreach (var plugin in pluginViewModels)
                {
                    plugin.IsInWishlist = await _wishlistRepository.IsPluginInWishlistAsync(userId, plugin.Id);
                    plugin.IsInCart = await _shoppingCartRepository.IsPluginInCartAsync(userId, plugin.Id); 
                }
            }
            return View(nameof(Index), pluginViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var plugin = await _pluginRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<PluginViewModel>(plugin);
            viewModel.IsInWishlist = await _wishlistRepository.IsPluginInWishlistAsync(userId, plugin.Id);
            viewModel.IsInCart = await _shoppingCartRepository.IsPluginInCartAsync(userId, plugin.Id);
        
            return View(nameof(Details), viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new PluginViewModel();
            return View(nameof(Create), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PluginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    // Define the folder to save the image
                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image");

                    // Generate a unique file name
                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewModel.ImageFile.FileName);
                    var filePath = Path.Combine(imagesFolder, fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(stream);
                    }

                    viewModel.Image = fileName;
                }
                try
                {
                    var plugin = _mapper.Map<Plugin>(viewModel);
                    await _pluginRepository.InsertAsync(plugin);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }
                return View(nameof(Create), viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _pluginRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<PluginViewModel>(course);

            return View(nameof(Edit), viewModel);
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
                try
                {
                    var plugin = _mapper.Map<Plugin>(viewModel);
                    await _pluginRepository.UpdateAsync(plugin);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            return View(nameof(Edit), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            var plugin = await _pluginRepository.GetByIdAsync(id);
            if (plugin == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<PluginViewModel>(plugin);
            return View(nameof(Remove), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            try
            {
                await _pluginRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                return View(nameof(Remove), await _pluginRepository.GetByIdAsync(id));
            }
        }

    } 
}

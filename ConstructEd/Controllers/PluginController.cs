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
        private readonly IEnrollmentRepository _enrollmentRepository;

        public PluginController(IPluginRepository pluginRepository, IMapper mapper,
                                IWishListRepository wishlistRepository,
                                IShoppingCartRepository shoppingCartRepository ,IEnrollmentRepository enrollmentRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _wishlistRepository = wishlistRepository;
            _pluginRepository = pluginRepository;
            _mapper = mapper;
            _enrollmentRepository = enrollmentRepository; ;

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
                    plugin.IsEnrolled = await _enrollmentRepository.IsUserEnrolledInPluginAsync(userId, plugin.Id);

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
            viewModel.IsEnrolled = await _enrollmentRepository.IsUserEnrolledInPluginAsync(userId, id);
            viewModel.IsInWishlist = await _wishlistRepository.IsPluginInWishlistAsync(userId, plugin.Id);
            viewModel.IsInCart = await _shoppingCartRepository.IsPluginInCartAsync(userId, plugin.Id);
            return View(nameof(Details), viewModel);
        }

    } 
}

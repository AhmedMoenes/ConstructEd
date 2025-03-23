using AutoMapper;
using ConstructEd.Services;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace ConstructEd.Controllers
{
    public class RoleController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public RoleController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

    }
}

using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IContactFormRepository _contactFormRepository;
        private readonly IMapper _mapper;

        public ContactUsController(IContactFormRepository contactFormRepository, IMapper mapper)
        {
            _contactFormRepository = contactFormRepository;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var model = new ContactFormViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contactForm = _mapper.Map<ContactForm>(model);

                await _contactFormRepository.AddContactFormAsync(contactForm);

                return RedirectToAction("ContactSuccess");
            }

            return View(model);
        }
        public IActionResult ContactSuccess()
        {
            return View();
        }
    }
}

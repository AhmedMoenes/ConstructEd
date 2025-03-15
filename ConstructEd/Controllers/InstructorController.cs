using AutoMapper;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructEd.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IMapper _mapper;

        public InstructorController(IInstructorRepository instructorRepository, IMapper mapper)
        {
            _instructorRepository = instructorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            var instructorVM = _mapper.Map<List<InstructorViewModel>>(instructors);
            return View(nameof(Index), instructorVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var instructor = _mapper.Map<Instructor>(viewModel);
                    await _instructorRepository.InsertAsync(instructor);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            return View(nameof(Create), viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<InstructorViewModel>(instructor);
            return View(nameof(Edit), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InstructorViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var instructor = _mapper.Map<Instructor>(viewModel);
                    await _instructorRepository.UpdateAsync(instructor);
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
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<InstructorViewModel>(instructor);
            return View(nameof(Remove), viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            try
            {
                await _instructorRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                return View(nameof(Remove), await _instructorRepository.GetByIdAsync(id));
            }
        }
    }
}
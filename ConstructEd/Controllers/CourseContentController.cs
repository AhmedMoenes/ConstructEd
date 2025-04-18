﻿using AutoMapper;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConstructEd.Controllers
{
    public class CourseContentController : Controller
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseContentController(
            ICourseContentRepository courseContentRepository,
            ICourseRepository courseRepository,
            IMapper mapper)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "User,Instructor,Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var courseContent = await _courseContentRepository.GetByIdAsync(id);
            if (courseContent == null)
            {
                return NotFound();
            }
            ;
            var viewModel = _mapper.Map<CourseContentViewModel>(courseContent);

            ViewBag.CourseName = courseContent.Course?.Title;
            ViewBag.CourseID = courseContent.Course?.Id;

            return View(viewModel);
        }

    }
}
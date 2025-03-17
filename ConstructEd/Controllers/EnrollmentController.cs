using AutoMapper;

using ConstructEd.Models;

using ConstructEd.Repositories;

using ConstructEd.ViewModels;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using System.Threading.Tasks;

public class EnrollmentsController : Controller

{

    private readonly IEnrollmentRepository _enrollmentRepository;

    private readonly IMapper _mapper;

    public EnrollmentsController(IEnrollmentRepository enrollmentRepository, IMapper mapper)
    {

        _enrollmentRepository = enrollmentRepository;

        _mapper = mapper;

    }

}


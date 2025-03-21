using AutoMapper;
using ConstructEd.Models;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Payment
        // Payment to PaymentViewModel mapping (For displaying payments) // Fix?

        CreateMap<PaymentViewModel, Payment>()
      .ForMember(dest => dest.MaskedCardNumber, opt => opt.MapFrom(src =>
            !string.IsNullOrEmpty(src.CardNumber) && src.CardNumber.Length >= 4
            ? "**** **** **** " + src.CardNumber.Substring(src.CardNumber.Length - 4)
            : "**** **** **** ****")) // Ensure masking
             .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => PaymentStatus.Pending))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(_ => DateTime.UtcNow));


        #endregion

        #region ApplicationUser

        CreateMap<RegisterViewModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.ConfirmedPassword, opt => opt.DoNotValidate());

        CreateMap<InstructorViewModel, ApplicationUser>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
            .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.ConfirmedPassword, opt => opt.DoNotValidate());

        CreateMap<ApplicationUser, InstructorViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
            .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience));

        // Mapping Between ProfileVM and ApplicationUser

        CreateMap<ApplicationUser, ProfileViewModel>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src =>
                File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", src.UserName + ".jpg"))
                ? "/uploads/" + src.UserName + ".jpg"
                : "/Image/default-user.jpg"
            ));
        #endregion


    #region Enrollments

        // Map Enrollment → EnrollmentViewModel
        CreateMap<Enrollment, EnrollmentViewModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName)) // Map User's Full Name
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId ?? 0)) // Default 0 if null
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : "N/A"))
            .ForMember(dest => dest.PluginId, opt => opt.MapFrom(src => src.PluginId ?? 0)) // Default 0 if null
            .ForMember(dest => dest.PluginTitle, opt => opt.MapFrom(src => src.Plugin != null ? src.Plugin.Title : "N/A"))
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrollmentDate.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => (int)(src.Progress ?? 0))); // Convert Progress to int
    
        #endregion

    #region Plugin
    // Map from Plugin to PluginViewModel
    CreateMap<Plugin, PluginViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        // Map from PluginViewModel to Plugin
        CreateMap<PluginViewModel, Plugin>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
     #endregion

        #region Course
        CreateMap<CourseViewModel, Course>()
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
               .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
               .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
               .ForMember(dest => dest.CourseContents, opt => opt.MapFrom(src => src.CourseContents));

        // Map from Course to CourseViewModel
        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.CourseContents, opt => opt.MapFrom(src => src.CourseContents));
        #endregion

        #region Instructor
        // Map from Instructor to InstructorViewModel
        //CreateMap<Instructor, InstructorViewModel>()
        //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
        //    .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
        //    .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

        //// Map from InstructorViewModel to Instructor
        //CreateMap<InstructorViewModel, Instructor>()
        //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
        //    .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
        //    .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));
        #endregion

        #region CourseContent
        // Map from CourseContent to CourseContentViewModel
        CreateMap<CourseContent, CourseContentViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));

        // Map from CourseContentViewModel to CourseContent
        CreateMap<CourseContentViewModel, CourseContent>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));
        #endregion

        #region CourseDetails
        // Map from CourseDetailsViewModel to Course
        CreateMap<CourseDetailsViewModel, Course>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<Category>(src.Category)))
            .ForMember(dest => dest.CourseContents, opt => opt.MapFrom(src => src.CourseContents));

        // Map from Course to CourseDetailsViewModel
        CreateMap<Course, CourseDetailsViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.CourseContents, opt => opt.MapFrom(src => src.CourseContents));
        #endregion
    }
}

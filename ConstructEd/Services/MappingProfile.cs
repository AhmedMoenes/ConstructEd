using AutoMapper;
using ConstructEd.Data;
using ConstructEd.Models;
using ConstructEd.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region ApplicationUser Mappings

        CreateMap<RegisterViewModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.ConfirmedPassword, opt => opt.DoNotValidate());

        CreateMap<ApplicationUser, InstructorViewModel>()
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
     .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
     .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
     .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
     .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
     .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
     .ForMember(dest => dest.IsInstructor, opt => opt.MapFrom(src => src.IsInstructor)); 

        CreateMap<InstructorViewModel, ApplicationUser>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
            .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
            .ForMember(dest => dest.IsInstructor, opt => opt.MapFrom(src => src.IsInstructor)) 
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.ConfirmedPassword, opt => opt.DoNotValidate());

        CreateMap<ApplicationUser, ProfileViewModel>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src =>
                File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", src.UserName + ".jpg"))
                    ? "/uploads/" + src.UserName + ".jpg"
                    : "/Image/default-user.jpg"
            ));

        #endregion

        #region Payment Mappings

        CreateMap<PaymentViewModel, Payment>()
            .ForMember(dest => dest.MaskedCardNumber, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.CardNumber) && src.CardNumber.Length >= 4
                    ? "**** **** **** " + src.CardNumber.Substring(src.CardNumber.Length - 4)
                    : "**** **** **** ****"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => PaymentStatus.Pending))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        #endregion

        #region Enrollment Mappings

        CreateMap<Enrollment, EnrollmentViewModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId ?? 0))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : "N/A"))
            .ForMember(dest => dest.PluginId, opt => opt.MapFrom(src => src.PluginId ?? 0))
            .ForMember(dest => dest.PluginTitle, opt => opt.MapFrom(src => src.Plugin != null ? src.Plugin.Title : "N/A"))
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrollmentDate.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => (int)(src.Progress ?? 0)));

        #endregion

        #region Plugin Mappings

        CreateMap<Plugin, PluginViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        CreateMap<PluginViewModel, Plugin>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        #endregion

        #region Course Mappings

        CreateMap<CourseViewModel, Course>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.CourseContents, opt => opt.MapFrom(src => src.CourseContents));

        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.CourseContents, opt => opt.MapFrom(src => src.CourseContents));

        #endregion

        #region CourseContent Mappings

        // Mapping from CourseContent to CourseContentViewModel
        CreateMap<CourseContent, CourseContentViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));

        // Mapping from CourseContentViewModel to CourseContent
        CreateMap<CourseContentViewModel, CourseContent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId));

        #endregion

        #region CourseDetails Mappings

        CreateMap<CourseDetailsViewModel, Course>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<Category>(src.Category)))
            .ForMember(dest => dest.CourseContents, opt => opt.MapFrom(src => src.CourseContents));

        CreateMap<Course, CourseDetailsViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.CourseContents, opt => opt.MapFrom(src => src.CourseContents));

        #endregion

        #region Statistics Mappings

        CreateMap<DataContext, StatisticsViewModel>()
        .ForMember(dest => dest.TotalCourses, opt => opt.MapFrom(src => src.Courses.Count()))
        .ForMember(dest => dest.ActiveCourses, opt =>
         opt.MapFrom(src => src.Courses.Count(c => c.Enrollments.Any())))
        .ForMember(dest => dest.MostPopularCourse, opt => opt.MapFrom(src => src.Courses
        .OrderByDescending(c => c.Enrollments.Count)
        .Select(c => c.Title)
        .FirstOrDefault()))
        .ForMember(dest => dest.TotalPlugins, opt => opt.MapFrom(src => src.Plugins.Count()))
        .ForMember(dest => dest.ActivePlugins, opt => 
         opt.MapFrom(src => src.Plugins.Count(p => p.Enrollments.Any())))
        .ForMember(dest => dest.MostPopularPlugin, opt => opt.MapFrom(src => src.Plugins
        .OrderByDescending(p => p.Enrollments.Count)
        .Select(p => p.Title)
        .FirstOrDefault()))
        .ForMember(dest => dest.TotalEnrollments, opt => opt.MapFrom(src => src.Enrollments.Count()))
        .ForMember(dest => dest.CourseEnrollments, opt =>
         opt.MapFrom(src => src.Enrollments.Count(e => e.CourseId.HasValue)))
        .ForMember(dest => dest.PluginEnrollments, opt => 
         opt.MapFrom(src => src.Enrollments.Count(e => e.PluginId.HasValue)))
        .ForMember(dest => dest.TotalUsers, opt => opt.MapFrom(src => src.Users.Count()))
        .ForMember(dest => dest.ActiveUsers, opt => 
         opt.MapFrom(src => src.Users.Count(u => u.Enrollments.Any())))
        .ForMember(dest => dest.TotalInstructors, opt => 
         opt.MapFrom(src => src.Users.Count(u => u.IsInstructor)))
        .ForMember(dest => dest.TotalPayments, opt => opt.MapFrom(src => src.Payments.Count()))
        .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.Payments
        .Where(p => p.Status == PaymentStatus.Success)
        .Sum(p => p.Amount)));

        #endregion

        #region ContactUs
        // Map from ViewModel to Entity
        CreateMap<ContactFormViewModel, ContactForm>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) 
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) 
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) 
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject)) 
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message)); 

        // Map from Entity to ViewModel
        CreateMap<ContactForm, ContactFormViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) 
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject)) 
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

        #endregion

        #region Reviews
        CreateMap<CourseReview, CourseReviewViewModel>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        CreateMap<CourseReviewViewModel, CourseReview>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) 
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); 
        #endregion
    }
}
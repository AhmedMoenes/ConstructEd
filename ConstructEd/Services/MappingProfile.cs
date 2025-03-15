using AutoMapper;
using ConstructEd.Models;

using ConstructEd.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Payment to PaymentViewModel mapping (For displaying payments)
        CreateMap<Payment, PaymentViewModel>()
            .ForMember(dest => dest.CardNumber, opt => opt.Ignore()) // Don't expose full card number
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

        // PaymentViewModel to Payment mapping (For processing payments)
        CreateMap<PaymentViewModel, Payment>()
            .ForMember(dest => dest.PaymentId, opt => opt.Ignore()) // Auto-generated
            .ForMember(dest => dest.MaskedCardNumber, opt => opt.MapFrom(src => "**** **** **** " + src.CardNumber.Substring(src.CardNumber.Length - 4))) // Masking card number
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Pending)) // Default status before processing
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        #region old code from gayar
        //// Payment to PaymentViewModel mapping
        //CreateMap<Payment, PaymentViewModel>()
        //    .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate.ToString("yyyy-MM-dd HH:mm:ss")))
        //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
        //    .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Method.ToString()))
        //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
        //    .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
        //    .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
        //    .ForMember(dest => dest.RefundDate, opt => opt.MapFrom(src => src.RefundDate != null ? src.RefundDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : null))
        //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")))
        //    .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => src.ModifiedAt != null ? src.ModifiedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null));
        #endregion

        // Mappings for ApplicationUser and RegisterViewModel
        // Source --> Destination
        CreateMap<RegisterViewModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) 
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)) 
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate()) 
            .ForSourceMember(src => src.ConfirmedPassword, opt => opt.DoNotValidate()); 

        
    }
}
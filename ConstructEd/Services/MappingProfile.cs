﻿using AutoMapper;
using ConstructEd.Models;
using ConstructEd.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Payment
        // Payment to PaymentViewModel mapping (For displaying payments) // Fix?
        #region Payment
        CreateMap<Payment, PaymentViewModel>()
        .ForMember(dest => dest.TransactionID, opt => opt.MapFrom(src => src.TransactionID))
        .ForMember(dest => dest.CardNumber, opt => opt.Ignore()) // Don't expose full card number
        .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
        .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
        #endregion

        // PaymentViewModel to Payment mapping (For processing payments)
        CreateMap<PaymentViewModel, Payment>()
            .ForMember(dest => dest.MaskedCardNumber, opt => opt.MapFrom(src => "**** **** **** " + src.CardNumber.Substring(src.CardNumber.Length - 4))) // Masking card number
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Pending)) // Default status before processing
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

        #endregion

        #region Plugin

        CreateMap<Plugin, PluginViewModel>();

        CreateMap<PluginViewModel, Plugin>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())   // System-managed
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());  // System-managed

        #endregion
        #region Course
        // Map from CourseViewModel to Course
        CreateMap<CourseViewModel, Course>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        
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
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        #endregion

        #region Instructor
        // Map from Instructor to InstructorViewModel
        CreateMap<Instructor, InstructorViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

        // Map from InstructorViewModel to Instructor
        CreateMap<InstructorViewModel, Instructor>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        #endregion

        #region CourseContent

        #endregion
    }
}

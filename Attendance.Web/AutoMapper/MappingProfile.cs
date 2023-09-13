using Attendance.Web.Dtos;
using Attendance.Web.ViewModels;
using AutoMapper;
namespace Web.AutoMapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            //CreateMap<UserModel, UserModel>()
            //    .ForMember(des => des.Id,
            //    src =>
            //    src.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.FamilyName, src =>
            //    src.MapFrom(src => src.FamilyName + src.FirstName))
            //     .ForMember(dest => dest.FamilyName,
            //src => src.Ignore());
            CreateMap<UserModel, UserViewModel>()
              .ForMember(dest => dest.DateOfBirth,
              src =>
              src.MapFrom(src => src.DateOfBirth.ToString()))
              .ForMember(dest => dest.Gender,
              src =>
              src.MapFrom(src => src.Gender.ToString() == "Male" ? 0 : 1))
              .ForMember(dest => dest.DateOfBirth,
              src =>
              src.MapFrom(src => src.DateOfBirth.ToString())).ReverseMap();
            CreateMap<UserModel, AddUserViewModel>()
              .ForMember(dest => dest.FullName,
              src =>
              src.MapFrom(src => string.Concat(src.FirstName, " ", src.FamilyName)))
                    .ForMember(dest => dest.Id,
              src =>
              src.Ignore())
              .ForMember(dest => dest.DateOfBirth,
              src =>
              src.MapFrom(src => src.DateOfBirth.ToString()))
              .ReverseMap();


            CreateMap<LeaveTypeModel, LeaveTypeDto>().ReverseMap();

            CreateMap<UserModel, EditUserViewModel>()
              .ForMember(dest => dest.DateOfBirth,
              src =>
              src.MapFrom(src => src.DateOfBirth.ToString()))
              .ForMember(dest => dest.Gender,
              src =>
              src.MapFrom(src => src.Gender.ToString() == "Male" ? 0 : 1))
              .ForMember(dest => dest.Email,
              src =>
              src.MapFrom(src => src.UserName))
                .ReverseMap();
        }
    }
}
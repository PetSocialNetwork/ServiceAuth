using AutoMapper;
using Service_Auth.Entities;
using ServiceAuth.Domain.Entities;
using ServiceAuth.WebApi.Models.Requests;
using ServiceAuth.WebApi.Models.Responses;

namespace ServiceAuth.WebApi.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, RegisterResponse>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToString()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<RegisterRequest, Account >()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<LoginRequest, Account>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<ResetPasswordRequest, Account>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.NewPassword));
        }
    }
}

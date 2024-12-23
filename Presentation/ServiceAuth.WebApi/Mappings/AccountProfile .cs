using AutoMapper;
using ServiceAuth.Domain.Entities;
using ServiceAuth.WebApi.Models.Responses;
using ServiceAuth.WebApi.Services;

namespace ServiceAuth.WebApi.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile(ITokenService tokenService)
        {
            CreateMap<Account, LoginResponse>()
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => tokenService.GenerateToken(src)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}

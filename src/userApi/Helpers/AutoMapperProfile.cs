using AutoMapper;
using userApi.Entities;
using userApi.Models.Roles;
using userApi.Models.Users;
using WebApi.Models.Users;

namespace userApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User -> AuthenticateResponse
            CreateMap<UserEntity, AuthenticateResponse>();

            // RegisterRequest -> User
            CreateMap<RegisterRequest, UserEntity>();

            // UpdateRequest -> User
            CreateMap<UpdateRequest, UserEntity>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
            
            CreateMap<ChangeClaimRequest, ClaimEntity>();
        }
    }
}
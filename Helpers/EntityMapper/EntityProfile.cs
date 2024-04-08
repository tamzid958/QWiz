using AutoMapper;
using QWiz.Entities;
using QWiz.Helpers.EntityMapper.DTOs;

namespace QWiz.Helpers.EntityMapper;

public class EntityProfile : Profile
{
    public EntityProfile()
    {
        CreateMap<AppUserDto, AppUser>()
            .ForSourceMember(
                o => o.Role,
                c => c.DoNotValidate()
            )
            .AfterMap((s, d) =>
            {
                d.UserName = s.Email;
                d.NormalizedUserName = s.Email.ToUpper();
                d.NormalizedEmail = s.Email.ToUpper();
            })
            .ForAllMembers(
                opts =>
                    opts.Condition((_, _, srcMember) => srcMember != null)
            );

        CreateMap<UpdateAppUserDto, AppUser>()
            .ForAllMembers(
                opts =>
                    opts.Condition((_, _, srcMember) => srcMember != null)
            );
    }
}
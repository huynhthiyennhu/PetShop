using AutoMapper;
using Microsoft.CodeAnalysis.Options;
using PetShop.Models;
using PetShop.ViewModles;

namespace PetShop.Helpers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, User>();
            //.ForMember(u=>u.PasswordHash,Option=>Option.MapFrom(RegisterVM => RegisterVM.Password)).ReverseMap();
            CreateMap<User, RegisterVM>();
        }
    }
}

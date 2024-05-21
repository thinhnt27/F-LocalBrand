using AutoMapper;
using F_LocalBrand.Dtos;
using F_LocalBrand.Models;

namespace F_LocalBrand.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        {
            CreateMap<User, UserModel>().ReverseMap();
//            CreateMap<Materials, MaterialsModel>().ReverseMap();
        }
    }
}

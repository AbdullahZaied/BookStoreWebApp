using AutoMapper;
using BookStoreAPI.Models;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Data;
using Data.Access.Layer.Models;

namespace Business.Logic.Layer.Mapper
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<SignInModelBusiness, SignInModelApi>().ReverseMap();
            CreateMap<SignUpModelBusiness, SignUpModelApi>().ReverseMap();
            CreateMap<OrderModelBusiness, OrderModelApi>().ReverseMap();
            CreateMap<StockModelBusiness, StockModelApi>().ReverseMap();
        }
    }
}

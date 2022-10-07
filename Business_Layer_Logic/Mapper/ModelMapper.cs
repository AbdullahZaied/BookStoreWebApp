using AutoMapper;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Data;
using Data.Access.Layer.Models;

namespace Business.Logic.Layer.Mapper
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<Book, BookModel>().ReverseMap();
            CreateMap<SignInModelBusiness, SignInModelData>().ReverseMap();
            CreateMap<SignUpModelBusiness, SignUpModelData>().ReverseMap();
            CreateMap<OrderModelBusiness, OrderModelData>().ReverseMap();
            CreateMap<StockModelBusiness, Stock>().ReverseMap();
        }
    }
}

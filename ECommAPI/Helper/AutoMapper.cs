using AutoMapper;
using DataAccessLayer;
using ECommRepo.Models;

namespace ECommAPI.Helper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartModel>().ReverseMap();
        }
    }
}

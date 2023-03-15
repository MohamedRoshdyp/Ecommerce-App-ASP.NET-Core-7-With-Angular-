using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;

namespace Ecom.API.MappingProfiles
{
    public class MappingBasket:Profile
    {
        public MappingBasket()
        {
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        }
    }
}

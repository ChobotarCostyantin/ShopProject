using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Orders.BLL.Features.OrderShipping.DTOs.Requests;
using Orders.BLL.Features.OrderShipping.DTOs.Responces;

namespace Orders.BLL.Features.OrderShipping.Mappers
{
    public class OrderShippingProfile : Profile
    {
        public OrderShippingProfile()
        {
            CreateMap<CreateOrderShippingRequest, Domain.Models.OrderShipping>();
            CreateMap<Domain.Models.OrderShipping, OrderShippingDto>();
        }
    }
}
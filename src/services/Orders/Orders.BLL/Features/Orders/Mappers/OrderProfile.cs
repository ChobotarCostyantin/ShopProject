using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Orders.BLL.Features.Orders.DTOs.Requests;
using Orders.BLL.Features.Orders.DTOs.Responces;

namespace Orders.BLL.Features.Orders.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderRequest, Domain.Models.Order>();
            
            CreateMap<Domain.Models.Order, OrderDto>();
        }
    }
}
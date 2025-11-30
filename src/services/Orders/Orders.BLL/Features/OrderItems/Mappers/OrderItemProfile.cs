using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Orders.BLL.Features.OrderItems.DTOs.Requests;
using Orders.BLL.Features.OrderItems.DTOs.Responces;

namespace Orders.BLL.Features.OrderItems.Mappers
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<CreateOrderItemRequest, Domain.Models.OrderItem>();
            CreateMap<Domain.Models.OrderItem, OrderItemDto>();
        }
    }
}
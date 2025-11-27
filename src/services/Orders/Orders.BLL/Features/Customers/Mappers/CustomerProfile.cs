using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Orders.BLL.Features.Customers.DTOs.Requests;
using Orders.BLL.Features.Customers.DTOs.Responses;
using Orders.Domain.Models;

namespace Orders.BLL.Features.Customers.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerRequest, Customer>();

            CreateMap<Customer, CustomerDto>();
        }
    }
}
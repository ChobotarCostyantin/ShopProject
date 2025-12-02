using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.BLL.DTOs.ProductDetails.Requests;
using Catalog.BLL.DTOs.ProductDetails.Responces;
using Catalog.DAL.Models;

namespace Catalog.BLL.Mappers
{
    public class ProductDetailsProfile : Profile
    {
        public ProductDetailsProfile()
        {
            CreateMap<CreateProductDetailsRequest, ProductDetails>();
            CreateMap<ProductDetails, ProductDetailsDto>();
        }
    }
}
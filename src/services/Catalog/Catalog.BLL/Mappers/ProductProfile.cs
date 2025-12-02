using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.BLL.DTOs.Products.Requests;
using Catalog.BLL.DTOs.Products.Responces;
using Catalog.DAL.Models;

namespace Catalog.BLL.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag)));
        }
    }
}
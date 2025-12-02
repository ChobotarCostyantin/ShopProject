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
                // Використовуємо ForCtorParam для записів (records) з конструктором
                .ForCtorParam(nameof(ProductDto.Tags), opt => 
                    opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag).ToArray()));
        }
    }
}
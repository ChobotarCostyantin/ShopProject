using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.BLL.DTOs.Categories.Requests;
using Catalog.BLL.DTOs.Categories.Responces;
using Catalog.DAL.Models;

namespace Catalog.BLL.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
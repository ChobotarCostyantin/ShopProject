using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.BLL.DTOs.Tags.Requests;
using Catalog.BLL.DTOs.Tags.Responces;
using Catalog.DAL.Models;

namespace Catalog.BLL.Mappers
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<CreateTagRequest, Tag>();
            CreateMap<Tag, TagDto>();
        }
    }
}
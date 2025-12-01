using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Common;

namespace Catalog.BLL.DTOs.Categories.Requests
{
    public record GetCategoriesRequest(string? Name) : GetRequest;
}
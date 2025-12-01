using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BLL.DTOs.Categories.Requests
{
    public record CreateCategoryRequest(
        string Name
    );
}
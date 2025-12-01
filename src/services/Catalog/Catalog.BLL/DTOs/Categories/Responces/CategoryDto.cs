using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BLL.DTOs.Categories.Responces
{
    public record CategoryDto(
        Guid CategoryId,
        string Name
    );
}
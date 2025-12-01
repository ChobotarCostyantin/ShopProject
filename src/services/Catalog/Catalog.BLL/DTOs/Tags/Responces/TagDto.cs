using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BLL.DTOs.Tags.Responces
{
    public record TagDto(
        Guid TagId,
        string Name
    );
}
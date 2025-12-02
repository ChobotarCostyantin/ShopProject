using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Tags.Requests;
using Catalog.BLL.DTOs.Tags.Responces;
using Shared.DTOs;
using Shared.ErrorHandling;

namespace Catalog.BLL.Services.Interfaces
{
    public interface ITagService
    {
        Task<Result<TagDto?>> GetTagByIdAsync(Guid tagId, CancellationToken cancellationToken);
        Task<Result<PaginationResult<TagDto>>> GetTagsAsync(GetTagsRequest request, CancellationToken cancellationToken);
        Task<Result<TagDto>> CreateTagAsync(CreateTagRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteTagAsync(Guid tagId, CancellationToken cancellationToken);
    }
}
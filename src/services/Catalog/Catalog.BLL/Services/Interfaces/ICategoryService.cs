using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Categories.Requests;
using Catalog.BLL.DTOs.Categories.Responces;
using Shared.DTOs;
using Shared.ErrorHandling;

namespace Catalog.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryDto?>> GetByIdAsync(Guid CategoryId, CancellationToken cancellationToken);
        Task<Result<PaginationResult<CategoryDto>>> GetCategoriesAsync(GetCategoriesRequest request, CancellationToken cancellationToken);
        Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
        Task<Result<CategoryDto>> UpdateCategoryAsync(Guid CategoryId, UpdateCategoryRequest request, CancellationToken cancellationToken);
        Task<Result<CategoryDto>> DeleteCategoryAsync(Guid CategoryId, CancellationToken cancellationToken);
    }
}
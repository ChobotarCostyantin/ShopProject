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
        Task<Result<CategoryDto?>> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken);
        Task<Result<PaginationResult<CategoryDto>>> GetCategoriesAsync(GetCategoriesRequest request, CancellationToken cancellationToken);
        Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken);
    }
}
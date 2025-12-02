using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Products.Requests;
using Catalog.BLL.DTOs.Products.Responces;
using Shared.DTOs;
using Shared.ErrorHandling;

namespace Catalog.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<Result<ProductDto?>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken);
        Task<Result<PaginationResult<ProductDto>>> GetProductsAsync(GetProductsRequest request, CancellationToken cancellationToken);
        Task<Result<ProductDto>> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken);
        Task<Result<ProductDto>> UpdateProductAsync(Guid productId, UpdateProductRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteProductAsync(Guid productId, CancellationToken cancellationToken);
        Task<Result<bool>> AddTagToProductAsync(Guid productId, AddTagToProductRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> RemoveTagFromProductAsync(Guid productId, RemoveTagFromProductRequest request, CancellationToken cancellationToken);
    }
}
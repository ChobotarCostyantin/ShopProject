using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.ProductDetails.Requests;
using Catalog.BLL.DTOs.ProductDetails.Responces;
using Shared.ErrorHandling;

namespace Catalog.BLL.Services.Interfaces
{
    public interface IProductDetailsService
    {
        Task<Result<ProductDetailsDto?>> GetProductDetailsByIdAsync(Guid productDetailsId, CancellationToken cancellationToken);
        Task<Result<ProductDetailsDto>> CreateProductDetailsAsync(CreateProductDetailsRequest request, CancellationToken cancellationToken);
        Task<Result<ProductDetailsDto>> UpdateProductDetailsAsync(Guid productDetailsId, UpdateProductDetailsRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteProductDetailsAsync(Guid productDetailsId, CancellationToken cancellationToken);
    }
}
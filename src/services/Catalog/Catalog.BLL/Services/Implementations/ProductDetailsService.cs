using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using AutoMapper;
using Catalog.BLL.DTOs.ProductDetails.Requests;
using Catalog.BLL.DTOs.ProductDetails.Responces;
using Catalog.BLL.Services.Interfaces;
using Catalog.DAL.Models;
using Catalog.DAL.UOW.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.ErrorHandling;

namespace Catalog.BLL.Services.Implementations
{
    public class ProductDetailsService : IProductDetailsService
    {
        private readonly ILogger<ProductDetailsService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateProductDetailsRequest> _createProductDetailsRequestValidator;
        private readonly IValidator<UpdateProductDetailsRequest> _updateProductDetailsRequestValidator;

        public ProductDetailsService(
            ILogger<ProductDetailsService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateProductDetailsRequest> createProductDetailsRequestValidator,
            IValidator<UpdateProductDetailsRequest> updateProductDetailsRequestValidator
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createProductDetailsRequestValidator = createProductDetailsRequestValidator;
            _updateProductDetailsRequestValidator = updateProductDetailsRequestValidator;
        }

        public async Task<Result<ProductDetailsDto?>> GetProductDetailsByIdAsync(Guid productDetailsId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching product details with ID: {ProductDetailsId}.", productDetailsId);

            var productDetails = await _unitOfWork.ProductDetailsRepository.GetByIdAsync(productDetailsId, cancellationToken);

            if (productDetails == null)
            {
                _logger.LogInformation("Product details with ID {ProductDetailsId} not found.", productDetailsId);
                return Result<ProductDetailsDto?>.NotFound(key: productDetailsId, entityName: nameof(ProductDetails));
            }

            _logger.LogInformation("Successfully retrieved product details with ID: {ProductDetailsId}", productDetailsId);
            return Result<ProductDetailsDto?>.Ok(_mapper.Map<ProductDetailsDto>(productDetails));
        }

        public async Task<Result<ProductDetailsDto>> CreateProductDetailsAsync(CreateProductDetailsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new product details entry: {@Request}", request);

            var validationResult = await _createProductDetailsRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for creating product details: {Error}",
                    validationResult.Errors[0].ErrorMessage);

                return Result<ProductDetailsDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var productDetails = _mapper.Map<ProductDetails>(request);
            productDetails.DetailsId = Guid.CreateVersion7();

            await _unitOfWork.ProductDetailsRepository.AddAsync(productDetails, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully created product details with ID: {ProductDetailsId}", productDetails.DetailsId);
            return Result<ProductDetailsDto>.Ok(_mapper.Map<ProductDetailsDto>(productDetails));

        }

        public async Task<Result<ProductDetailsDto>> UpdateProductDetailsAsync(Guid productDetailsId, UpdateProductDetailsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating product details with ID: {ProductDetailsId} using {@Request}", productDetailsId, request);

            var productDetails = await _unitOfWork.ProductDetailsRepository.GetByIdAsync(productDetailsId, cancellationToken);
            if (productDetails == null)
            {
                _logger.LogWarning("Cannot update inventory. ID {ProductDetailsId} not found", productDetailsId);
                return Result<ProductDetailsDto>.NotFound(key: productDetailsId, entityName: nameof(ProductDetails));
            }

            var validationResult = await _updateProductDetailsRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for updating product details: {Error}",
                    validationResult.Errors[0].ErrorMessage);

                return Result<ProductDetailsDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            productDetails.Description = request.Description;
            productDetails.Manufacturer = request.Manufacturer;
            productDetails.Weight_Kg = request.Weight_Kg;

            await _unitOfWork.ProductDetailsRepository.UpdateAsync(productDetails);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product details with ID {productDetailsId} updated successfully", productDetailsId);

            return Result<ProductDetailsDto>.Ok(_mapper.Map<ProductDetailsDto>(productDetails));
        }

        public async Task<Result<bool>> DeleteProductDetailsAsync(Guid productDetailsId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete product details with ID: {productDetailsId}", productDetailsId);

            var productDetails = await _unitOfWork.ProductDetailsRepository.GetByIdAsync(productDetailsId, cancellationToken);
            if (productDetails == null)
            {
                _logger.LogInformation("Cannot delete product details. ID {productDetailsId} not found.", productDetailsId);
                return Result<bool>.NotFound(key: productDetailsId, entityName: nameof(ProductDetails));
            }

            await _unitOfWork.ProductDetailsRepository.RemoveAsync(productDetails);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product details with ID {productDetailsId} deleted successfully", productDetailsId);
            return Result<bool>.NoContent();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ardalis.Specification;
using AutoMapper;
using Catalog.BLL.DTOs.Products.Requests;
using Catalog.BLL.DTOs.Products.Responces;
using Catalog.BLL.Services.Interfaces;
using Catalog.BLL.Specifications;
using Catalog.DAL.Models;
using Catalog.DAL.UOW.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.ErrorHandling;

namespace Catalog.BLL.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<AddTagToProductRequest> _addTagToProductRequestValidator;
        private readonly IValidator<CreateProductRequest> _createProductRequestValidator;
        private readonly IValidator<RemoveTagFromProductRequest> _removeTagFromProductRequestValidator;
        private readonly IValidator<UpdateProductRequest> _updateProductRequestValidator;

        public ProductService(
            ILogger<ProductService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<AddTagToProductRequest> addTagToProductRequestValidator,
            IValidator<CreateProductRequest> createProductRequestValidator,
            IValidator<RemoveTagFromProductRequest> removeTagFromProductRequestValidator,
            IValidator<UpdateProductRequest> updateProductRequestValidator
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _addTagToProductRequestValidator = addTagToProductRequestValidator;
            _createProductRequestValidator = createProductRequestValidator;
            _removeTagFromProductRequestValidator = removeTagFromProductRequestValidator;
            _updateProductRequestValidator = updateProductRequestValidator;
        }

        public async Task<Result<ProductDto?>> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching product with ID: {ProductId}.", productId);

            var product = await _unitOfWork.ProductRepository.GetProductWithRelatedEntitiesAsync(
                productId,
                true,
                true,
                true,
                cancellationToken
            );

            if (product == null)
            {
                _logger.LogInformation("Product with ID {ProductId} not found.", productId);
                return Result<ProductDto?>.NotFound(key: productId, entityName: nameof(Product));
            }

            _logger.LogInformation("Successfully retrieved product with ID: {ProductId}", productId);
            return Result<ProductDto?>.Ok(_mapper.Map<ProductDto>(product));
        }

        public async Task<Result<PaginationResult<ProductDto>>> GetProductsAsync(GetProductsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching products with filters: {@Request}", request);

            var specification = new ProductSpecification(request);
            var products = (await _unitOfWork.ProductRepository.ListBySpecAsync(specification, cancellationToken)).ToList();
            var totalCount = await _unitOfWork.ProductRepository.CountBySpecAsync(new ProductSpecification(request, true), cancellationToken);

            _logger.LogInformation("Fetched {Count} products (Page {PageNumber}, PageSize {PageSize})",
                products.Count, request.PageNumber, request.PageSize);

            return Result<PaginationResult<ProductDto>>.Ok(new PaginationResult<ProductDto>(
                products.Select(_mapper.Map<ProductDto>).ToArray(),
                totalCount,
                request.PageNumber,
                Math.Ceiling((decimal)totalCount / request.PageSize),
                request.PageSize));
        }

        public async Task<Result<ProductDto>> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new product entry: {@Request}", request);

            var validationResult = await _createProductRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for creating product: {Error}",
                    validationResult.Errors[0].ErrorMessage);

                return Result<ProductDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var product = _mapper.Map<Product>(request);
            product.ProductId = Guid.CreateVersion7();
            product.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully created product with ID: {ProductId}", product.ProductId);
            return Result<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
        }

        public async Task<Result<ProductDto>> UpdateProductAsync(Guid productId, UpdateProductRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}.", productId);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                _logger.LogInformation("Product with ID {ProductId} not found.", productId);
                return Result<ProductDto>.NotFound(key: productId, entityName: nameof(Product));
            }

            var validationResult = await _updateProductRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for updating product: {Error}",
                    validationResult.Errors[0].ErrorMessage);

                return Result<ProductDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            _mapper.Map(request, product);
            product.CategoryId = request.CategoryId;
            product.Name = request.Name;
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;

            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated product with ID: {ProductId}", product.ProductId);
            return Result<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
        }

        public async Task<Result<bool>> DeleteProductAsync(Guid productId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete product with ID: {ProductId}.", productId);

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                _logger.LogInformation("Cannot delete product. ID {ProductId} not found.", productId);
                return Result<bool>.NotFound(key: productId, entityName: nameof(Product));
            }

            await _unitOfWork.ProductRepository.RemoveAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product with ID {ProductId} deleted successfully", productId);
            return Result<bool>.NoContent();
        }

        public async Task<Result<bool>> AddTagToProductAsync(Guid productId, AddTagToProductRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding tag to product with ID: {ProductId}.", productId);

            var validationResult = await _addTagToProductRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for adding tag to product: {Error}",
                    validationResult.Errors[0].ErrorMessage);

                return Result<bool>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                _logger.LogInformation("Product with ID {ProductId} not found.", productId);
                return Result<bool>.NotFound(key: productId, entityName: nameof(Product));
            }

            var tag = await _unitOfWork.TagRepository.GetByIdAsync(request.TagId, cancellationToken);
            if (tag == null)
            {
                _logger.LogInformation("Tag with ID {TagId} not found.", request.TagId);
                return Result<bool>.NotFound(key: request.TagId, entityName: nameof(Tag));
            }

            var existingLink = await _unitOfWork.ProductTagRepository.FindAsync(
                pt => pt.ProductId == productId && pt.TagId == request.TagId, 
                pageSize: 1, 
                pageNumber: 1, 
                cancellationToken
            );

            if (existingLink.Any())
            {
                _logger.LogInformation("Tag with ID {TagId} is already linked to product with ID {ProductId}.", request.TagId, productId);

                return Result<bool>.Conflict($"Tag with ID {request.TagId} is already linked to product with ID {productId}.");
            }

            var productTag = new ProductTag
            {
                ProductId = productId,
                TagId = request.TagId
            };

            await _unitOfWork.ProductTagRepository.AddAsync(productTag, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added tag with ID {TagId} to product with ID {ProductId}", request.TagId, productId);
            return Result<bool>.NoContent();
        }

        public async Task<Result<bool>> RemoveTagFromProductAsync(Guid productId, RemoveTagFromProductRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to remove tag from product with ID: {ProductId}.", productId);

            var validationResult = await _removeTagFromProductRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for removing tag from product: {Error}",
                    validationResult.Errors[0].ErrorMessage);

                return Result<bool>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                _logger.LogInformation("Product with ID {ProductId} not found.", productId);
                return Result<bool>.NotFound(key: productId, entityName: nameof(Product));
            }

            var tag = await _unitOfWork.TagRepository.GetByIdAsync(request.TagId, cancellationToken);
            if (tag == null)
            {
                _logger.LogInformation("Tag with ID {TagId} not found.", request.TagId);
                return Result<bool>.NotFound(key: request.TagId, entityName: nameof(Tag));
            }

            var existingLink = await _unitOfWork.ProductTagRepository.FindAsync(
                pt => pt.ProductId == productId && pt.TagId == request.TagId,
                pageSize: 1,
                pageNumber: 1,
                cancellationToken
            );

            if (!existingLink.Any())
            {
                _logger.LogInformation("Tag with ID {TagId} is not linked to product with ID {ProductId}.", request.TagId, productId);
                return Result<bool>.NotFound(key: request.TagId, entityName: nameof(ProductTag));
            }

            await _unitOfWork.ProductTagRepository.RemoveAsync(existingLink.First());
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tag with ID {TagId} from product with ID {ProductId} removed successfully", request.TagId, productId);
            return Result<bool>.NoContent();
        }
    }
}
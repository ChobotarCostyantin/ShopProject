using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.BLL.DTOs.Categories.Requests;
using Catalog.BLL.DTOs.Categories.Responces;
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
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateCategoryRequest> _createCategoryRequestValidator;
        
        public CategoryService(
            ILogger<CategoryService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateCategoryRequest> createCategoryRequestValidator
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createCategoryRequestValidator = createCategoryRequestValidator;
        }
        public async Task<Result<CategoryDto?>> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching category with ID: {CategoryId}.", categoryId);

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category == null)
            {
                _logger.LogInformation("Category with ID {CategoryId} not found.", categoryId);
                return Result<CategoryDto?>.NotFound(key: categoryId, entityName: nameof(Category));
            }

            _logger.LogInformation("Successfully retrieved category with ID: {CategoryId}", categoryId);
            return Result<CategoryDto?>.Ok(_mapper.Map<CategoryDto>(category));
        }

        public async Task<Result<PaginationResult<CategoryDto>>> GetCategoriesAsync(GetCategoriesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching categories with filters: {@Request}", request);

            var specification = new CategorySpecification(request);
            var categories = (await _unitOfWork.CategoryRepository.ListBySpecAsync(specification, cancellationToken)).ToList();
            var totalCount = await _unitOfWork.CategoryRepository.CountBySpecAsync(new CategorySpecification(request, true), cancellationToken);

            _logger.LogInformation("Fetched {Count} categories (Page {PageNumber}, PageSize {PageSize})",
                categories.Count, request.PageNumber, request.PageSize);

            return Result<PaginationResult<CategoryDto>>.Ok(new PaginationResult<CategoryDto>(
                categories.Select(_mapper.Map<CategoryDto>).ToArray(),
                totalCount,
                request.PageNumber,
                Math.Ceiling((decimal)totalCount / request.PageSize),
                request.PageSize));
        }

        public async Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new category entry: {@Request}", request);

            var validationResult = await _createCategoryRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for creating category: {Error}",
                    validationResult.Errors[0].ErrorMessage);

                return Result<CategoryDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var category = _mapper.Map<Category>(request);
            category.CategoryId = Guid.CreateVersion7();

            await _unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category created successfully with ID: {categoryId}", category.CategoryId);
            return Result<CategoryDto>.Ok(_mapper.Map<CategoryDto>(category));
        }

        public async Task<Result<bool>> DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete category with ID: {categoryId}", categoryId);

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category == null)
            {
                _logger.LogInformation("Cannot delete category. ID {categoryId} not found.", categoryId);
                return Result<bool>.NotFound(key: categoryId, entityName: nameof(Category));
            }

            await _unitOfWork.CategoryRepository.RemoveAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Category with ID {categoryId} deleted successfully", categoryId);
            return Result<bool>.NoContent();
        }
    }
}
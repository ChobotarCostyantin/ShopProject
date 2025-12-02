using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using AutoMapper;
using Catalog.BLL.DTOs.Tags.Requests;
using Catalog.BLL.DTOs.Tags.Responces;
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
    public class TagService : ITagService
    {
        private readonly ILogger<TagService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateTagRequest> _createTagRequestValidator;

        public TagService(
            ILogger<TagService> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateTagRequest> createTagRequestValidator
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createTagRequestValidator = createTagRequestValidator;
        }

        public async Task<Result<TagDto?>> GetTagByIdAsync(Guid tagId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching tag with ID: {TagId}.", tagId);

            var tag = await _unitOfWork.TagRepository.GetByIdAsync(tagId, cancellationToken);
            if (tag == null)
            {
                _logger.LogInformation("Tag with ID {TagId} not found.", tagId);
                return Result<TagDto?>.NotFound(key: tagId, entityName: nameof(Tag));
            }

            _logger.LogInformation("Successfully retrieved tag with ID: {TagId}", tagId);
            return Result<TagDto?>.Ok(_mapper.Map<TagDto>(tag));
        }

        public async Task<Result<PaginationResult<TagDto>>> GetTagsAsync(GetTagsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching tags with filters: {@Request}", request);

            var specification = new TagSpecification(request);
            var tags = (await _unitOfWork.TagRepository.ListBySpecAsync(specification, cancellationToken)).ToList();
            var totalCount = await _unitOfWork.TagRepository.CountBySpecAsync(new TagSpecification(request, true), cancellationToken);

            _logger.LogInformation("Fetched {Count} tags (Page {PageNumber}, PageSize {PageSize})",
                tags.Count, request.PageNumber, request.PageSize);

            return Result<PaginationResult<TagDto>>.Ok(new PaginationResult<TagDto>(
                tags.Select(_mapper.Map<TagDto>).ToArray(),
                totalCount,
                request.PageNumber,
                Math.Ceiling((decimal)totalCount / request.PageSize),
                request.PageSize));
        }

        public async Task<Result<TagDto>> CreateTagAsync(CreateTagRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new tag entry: {@Request}", request);

            var validationResult = await _createTagRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for creating tag: {Error}",
                    validationResult.Errors[0].ErrorMessage);

                return Result<TagDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var tag = _mapper.Map<Tag>(request);
            tag.TagId = Guid.CreateVersion7();

            await _unitOfWork.TagRepository.AddAsync(tag, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully created tag with ID: {TagId}", tag.TagId);
            return Result<TagDto>.Ok(_mapper.Map<TagDto>(tag));
        }

        public async Task<Result<bool>> DeleteTagAsync(Guid tagId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete tag with ID: {tagId}", tagId);

            var tag = await _unitOfWork.TagRepository.GetByIdAsync(tagId, cancellationToken);
            if (tag == null)
            {
                _logger.LogInformation("Cannot delete tag. ID {tagId} not found.", tagId);
                return Result<bool>.NotFound(key: tagId, entityName: nameof(Tag));
            }

            await _unitOfWork.TagRepository.RemoveAsync(tag);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tag with ID {tagId} deleted successfully", tagId);
            return Result<bool>.NoContent();
        }
    }
}
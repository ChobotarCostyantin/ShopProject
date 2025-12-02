using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Tags.Requests;
using Catalog.BLL.DTOs.Tags.Responces;
using Catalog.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    public class TagsController : BaseApiController
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("{tagId:guid}")]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid tagId, CancellationToken cancellationToken)
        {
            return (await _tagService.GetTagByIdAsync(tagId, cancellationToken)).ToApiResponse();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResult<TagDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromQuery] GetTagsRequest request, CancellationToken cancellationToken)
        {
            return (await _tagService.GetTagsAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(typeof(TagDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostAsync(CreateTagRequest request, CancellationToken cancellationToken)
        {
            return (await _tagService.CreateTagAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{tagId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid tagId, CancellationToken cancellationToken)
        {
            return (await _tagService.DeleteTagAsync(tagId, cancellationToken)).ToApiResponse();
        }
    }
}
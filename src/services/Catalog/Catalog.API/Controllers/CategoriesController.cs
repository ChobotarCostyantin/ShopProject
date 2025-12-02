using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Categories.Requests;
using Catalog.BLL.DTOs.Categories.Responces;
using Catalog.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{categoryId:guid}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return (await _categoryService.GetCategoryByIdAsync(categoryId, cancellationToken)).ToApiResponse();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResult<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromQuery] GetCategoriesRequest request, CancellationToken cancellationToken)
        {
            return (await _categoryService.GetCategoriesAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            return (await _categoryService.CreateCategoryAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{categoryId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return (await _categoryService.DeleteCategoryAsync(categoryId, cancellationToken)).ToApiResponse();
        }
    }
}
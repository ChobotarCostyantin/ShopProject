using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Products.Requests;
using Catalog.BLL.DTOs.Products.Responces;
using Catalog.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{productId:guid}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            return (await _productService.GetProductByIdAsync(productId, cancellationToken)).ToApiResponse();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResult<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromQuery] GetProductsRequest request, CancellationToken cancellationToken)
        {
            return (await _productService.GetProductsAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostAsync(CreateProductRequest request, CancellationToken cancellationToken)
        {
            return (await _productService.CreateProductAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPut("{productId:guid}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PutAsync(Guid productId, UpdateProductRequest request, CancellationToken cancellationToken)
        {
            return (await _productService.UpdateProductAsync(productId, request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid productId, CancellationToken cancellationToken)
        {
            return (await _productService.DeleteProductAsync(productId, cancellationToken)).ToApiResponse();
        }

        [HttpPost("{productId:guid}/tags")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddTagAsync(Guid productId, AddTagToProductRequest request, CancellationToken cancellationToken)
        {
            return (await _productService.AddTagToProductAsync(productId, request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{productId:guid}/tags")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveTagAsync(Guid productId, RemoveTagFromProductRequest request, CancellationToken cancellationToken)
        {
            return (await _productService.RemoveTagFromProductAsync(productId, request, cancellationToken)).ToApiResponse();
        }
    }
}
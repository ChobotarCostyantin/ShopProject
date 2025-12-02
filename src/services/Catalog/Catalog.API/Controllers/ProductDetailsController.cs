using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.ProductDetails.Requests;
using Catalog.BLL.DTOs.ProductDetails.Responces;
using Catalog.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductDetailsController : BaseApiController
    {
        private readonly IProductDetailsService _productDetailsService;

        public ProductDetailsController(IProductDetailsService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        [HttpGet("{productDetailsId:guid}")]
        [ProducesResponseType(typeof(ProductDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid productDetailsId, CancellationToken cancellationToken)
        {
            return (await _productDetailsService.GetProductDetailsByIdAsync(productDetailsId, cancellationToken)).ToApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDetailsDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostAsync(CreateProductDetailsRequest request, CancellationToken cancellationToken)
        {
            return (await _productDetailsService.CreateProductDetailsAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPut("{productDetailsId:guid}")]
        [ProducesResponseType(typeof(ProductDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PutAsync(Guid productDetailsId, UpdateProductDetailsRequest request, CancellationToken cancellationToken)
        {
            return (await _productDetailsService.UpdateProductDetailsAsync(productDetailsId, request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{productDetailsId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid productDetailsId, CancellationToken cancellationToken)
        {
            return (await _productDetailsService.DeleteProductDetailsAsync(productDetailsId, cancellationToken)).ToApiResponse();
        }
    }
}
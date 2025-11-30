using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orders.BLL.Features.OrderShipping.DTOs.Requests;
using Orders.BLL.Features.OrderShipping.DTOs.Responces;
using Orders.BLL.Features.OrderShipping.Services.Interfaces;

namespace Orders.API.Controllers
{
    [Route("api/[controller]")]
    public class OrderShippingsController : BaseApiController
    {
        private readonly IOrderShippingService _orderShippingService;

        public OrderShippingsController(IOrderShippingService orderShippingService)
        {
            _orderShippingService = orderShippingService;
        }

        [HttpGet("{shippingId:guid}")] 
        [ProducesResponseType(typeof (OrderShippingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid shippingId, CancellationToken cancellationToken)
        {
            return (await _orderShippingService.GetOrderShippingByIdAsync(new GetOrderShippingByIdRequest(shippingId), cancellationToken)).ToApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(typeof (OrderShippingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(CreateOrderShippingRequest request, CancellationToken cancellationToken)
        {
            return (await _orderShippingService.CreateOrderShippingAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPut("{shippingId:guid}")] 
        [ProducesResponseType(typeof (OrderShippingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(Guid shippingId, UpdateOrderShippingRequest request, CancellationToken cancellationToken)
        {
            return (await _orderShippingService.UpdateOrderShippingAsync(shippingId, request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{shippingId:guid}")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid shippingId, CancellationToken cancellationToken)
        {
            return (await _orderShippingService.DeleteOrderShippingAsync(new DeleteOrderShippingRequest(shippingId), cancellationToken)).ToApiResponse();
        }
    }
}
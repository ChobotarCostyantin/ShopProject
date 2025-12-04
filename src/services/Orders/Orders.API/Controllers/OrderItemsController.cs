using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orders.BLL.Features.OrderItems.DTOs.Requests;
using Orders.BLL.Features.OrderItems.DTOs.Responces;
using Orders.BLL.Features.OrderItems.Services.Interfaces;

namespace Orders.API.Controllers
{
    [Route("api/[controller]")]
    public class OrderItemsController : BaseApiController
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("{orderItemId:guid}")]
        [ProducesResponseType(typeof (OrderItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid orderItemId, CancellationToken cancellationToken)
        {
            return (await _orderItemService.GetOrderItemByIdAsync(orderItemId, cancellationToken)).ToApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(typeof (OrderItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(CreateOrderItemRequest request, CancellationToken cancellationToken)
        {
            return (await _orderItemService.CreateOrderItemAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPut("{orderItemId:guid}")] 
        [ProducesResponseType(typeof (OrderItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(Guid orderItemId, UpdateOrderItemRequest request, CancellationToken cancellationToken)
        {
            return (await _orderItemService.UpdateOrderItemAsync(orderItemId, request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{orderItemId:guid}")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid orderItemId, CancellationToken cancellationToken)
        {
            return (await _orderItemService.DeleteOrderItemAsync(new DeleteOrderItemRequest(orderItemId), cancellationToken)).ToApiResponse();
        }
    }
}
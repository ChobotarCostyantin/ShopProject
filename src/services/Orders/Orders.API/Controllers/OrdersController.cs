using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orders.BLL.Features.Customers.Services.interfaces;
using Orders.BLL.Features.OrderItems.DTOs.Requests;
using Orders.BLL.Features.OrderItems.DTOs.Responces;
using Orders.BLL.Features.OrderItems.Services.Interfaces;
using Orders.BLL.Features.Orders.DTOs.Requests;
using Orders.BLL.Features.Orders.DTOs.Responces;
using Orders.BLL.Features.Orders.Services.Interfaces;
using Orders.BLL.Features.OrderShipping.DTOs.Requests;
using Orders.BLL.Features.OrderShipping.DTOs.Responces;
using Orders.BLL.Features.OrderShipping.Services.Interfaces;
using Shared.DTOs;

namespace Orders.API.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderShippingService _orderShippingService;

        public OrdersController(IOrderService orderService, IOrderItemService orderItemService, IOrderShippingService orderShippingService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _orderShippingService = orderShippingService;
        }

        [HttpGet("{customerId:guid}/orders")]
        [ProducesResponseType(typeof (PaginationResult<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid customerId, [FromQuery] GetOrdersByCustomerIdRequest request, CancellationToken cancellationToken)
        {
            return (await _orderService.GetOrdersByCustomerIdAsync(customerId, request, cancellationToken)).ToApiResponse();
        }

        [HttpGet("{orderId:guid}")] 
        [ProducesResponseType(typeof (OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return (await _orderService.GetOrderByIdAsync(new GetOrderByIdRequest(orderId), cancellationToken)).ToApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(typeof (OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            return (await _orderService.CreateOrderAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPut("{orderId:guid}")] 
        [ProducesResponseType(typeof (OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            return (await _orderService.UpdateOrderAsync(orderId, request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{orderId:guid}")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return (await _orderService.DeleteOrderAsync(new DeleteOrderRequest(orderId), cancellationToken)).ToApiResponse();
        }

        [HttpGet("{orderId:guid}/orderItems")] 
        [ProducesResponseType(typeof (PaginationResult<OrderItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderItemsAsync([FromRoute] Guid orderId, [FromQuery] GetOrderItemsByOrderIdRequest request, CancellationToken cancellationToken)
        {
            return (await _orderItemService.GetOrderItemsByOrderIdAsync(orderId, request, cancellationToken)).ToApiResponse();
        }

        [HttpGet("{orderId:guid}/shipping")] 
        [ProducesResponseType(typeof (OrderShippingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderShippingAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return (await _orderShippingService.GetOrderShippingByOrderIdAsync(new GetOrderShippingByOrderIdRequest(orderId), cancellationToken)).ToApiResponse();
        }
    }
}
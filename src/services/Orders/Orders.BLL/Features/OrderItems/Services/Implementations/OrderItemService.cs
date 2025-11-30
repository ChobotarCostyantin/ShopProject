using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Orders.BLL.Features.OrderItems.DTOs.Requests;
using Orders.BLL.Features.OrderItems.DTOs.Responces;
using Orders.BLL.Features.OrderItems.Services.Interfaces;
using Orders.DAL.Repositories.UOW.Interfaces;
using Shared.DTOs;
using Shared.ErrorHandling;
using Shared.Exceptions;

namespace Orders.BLL.Features.OrderItems.Services.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateOrderItemRequest> _createOrderItemRequestValidator;
        private readonly ILogger<OrderItemService> _logger;
        private readonly IMapper _mapper;

        public OrderItemService(IUnitOfWork unitOfWork, IValidator<CreateOrderItemRequest> createOrderItemRequestValidator, ILogger<OrderItemService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _createOrderItemRequestValidator = createOrderItemRequestValidator;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<OrderItemDto>> GetOrderItemByIdAsync(GetOrderItemByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderItem = await _unitOfWork.OrderItemRepository.GetOrderItemAsync(request.OrderItemId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();
                
                return orderItem is null
                    ? Result<OrderItemDto>.NotFound(key: request.OrderItemId, entityName: nameof(Domain.Models.OrderItem))
                    : Result<OrderItemDto>.Ok(_mapper.Map<OrderItemDto>(orderItem));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<OrderItemDto>> CreateOrderItemAsync(CreateOrderItemRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _createOrderItemRequestValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result<OrderItemDto>.NotFound(alternativeMessage: $"Product with id of {request.ProductId} was not found");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var order = await _unitOfWork.OrderRepository.GetOrderAsync(request.OrderId, cancellationToken);
                if (order is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<OrderItemDto>.NotFound(key: request.OrderId, entityName: nameof(Domain.Models.Order));
                }

                var orderItem = _mapper.Map<Domain.Models.OrderItem>(request);
                orderItem.OrderItemId = Guid.CreateVersion7();

                await _unitOfWork.OrderItemRepository.CreateOrderItemAsync(orderItem, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<OrderItemDto>.Created($"/api/order-items/{orderItem.OrderItemId}", _mapper.Map<OrderItemDto>(orderItem));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<OrderItemDto>> UpdateOrderItemAsync(Guid orderItemId, UpdateOrderItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderItem = await _unitOfWork.OrderItemRepository.GetOrderItemAsync(orderItemId, cancellationToken);
                if (orderItem is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<OrderItemDto>.NotFound(key: orderItemId, entityName: nameof(Domain.Models.OrderItem));
                }

                orderItem.Quantity = request.Quantity;

                await _unitOfWork.OrderItemRepository.UpdateOrderItemAsync(orderItemId, orderItem, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<OrderItemDto>.Ok(_mapper.Map<OrderItemDto>(orderItem));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<bool>> DeleteOrderItemAsync(DeleteOrderItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderItem = await _unitOfWork.OrderItemRepository.GetOrderItemAsync(request.OrderItemId, cancellationToken);
                if (orderItem is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<bool>.NotFound(key: request.OrderItemId, entityName: nameof(Domain.Models.OrderItem));
                }

                await _unitOfWork.OrderItemRepository.DeleteOrderItemAsync(request.OrderItemId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<bool>.NoContent();
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }
        
        public async Task<Result<PaginationResult<OrderItemDto>>> GetOrderItemsByOrderIdAsync(Guid orderId, GetOrderItemsByOrderIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderItems = await _unitOfWork.OrderItemRepository.GetOrderItemsAsync(orderId, request.PageSize, request.PageNumber, cancellationToken);
                var totalCount = await _unitOfWork.OrderItemRepository.CountAllInOrderAsync(orderId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<PaginationResult<OrderItemDto>>.Ok(new PaginationResult<OrderItemDto>(
                    orderItems.Select(x => _mapper.Map<OrderItemDto>(x)).ToArray(),
                    totalCount,
                    request.PageNumber,
                    Math.Ceiling((decimal)totalCount / request.PageSize),
                    request.PageSize
                ));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }
    }
}
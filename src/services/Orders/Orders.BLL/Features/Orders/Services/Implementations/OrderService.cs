using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Orders.BLL.Features.Orders.DTOs.Requests;
using Orders.BLL.Features.Orders.DTOs.Responces;
using Orders.BLL.Features.Orders.Services.Interfaces;
using Orders.DAL.Repositories.UOW.Interfaces;
using Shared.DTOs;
using Shared.ErrorHandling;
using Shared.Exceptions;

namespace Orders.BLL.Features.Orders.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto?>> GetOrderByIdAsync(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var order = await _unitOfWork.OrderRepository.GetOrderAsync(request.OrderId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return order is null
                    ? Result<OrderDto?>.NotFound(key: request.OrderId, entityName: nameof(Domain.Models.Order))
                    : Result<OrderDto?>.Ok(_mapper.Map<OrderDto>(order));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<PaginationResult<OrderDto>>> GetOrdersByCustomerIdAsync(Guid customerId, GetOrdersByCustomerIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orders = await _unitOfWork.OrderRepository.GetOrdersByCustomerIdAsync(customerId, request.PageSize, request.PageNumber, cancellationToken);
                var totalCount = await _unitOfWork.OrderRepository.CountAllOrdersByCustomerIdAsync(customerId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<PaginationResult<OrderDto>>.Ok(new PaginationResult<OrderDto>(
                    orders.Select(x => _mapper.Map<OrderDto>(x)).ToArray(),
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

        public async Task<Result<OrderDto>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var order = _mapper.Map<Domain.Models.Order>(request);
                order.OrderId = Guid.CreateVersion7();
                order.TotalPrice = 0; //order.OrderItems.Sum(x => x.UnitPrice * x.Quantity);
                order.Status = Domain.Enums.Status.New;
                order.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.OrderRepository.CreateOrderAsync(order, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<OrderDto>.Created($"/api/orders/{order.OrderId}", _mapper.Map<OrderDto>(order));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<OrderDto>> UpdateOrderAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var order = await _unitOfWork.OrderRepository.GetOrderAsync(orderId, cancellationToken);
                if (order is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<OrderDto>.NotFound(key: orderId, entityName: nameof(Domain.Models.Order));
                }

                order.DeliveryDate = request.DeliveryDate;
                order.Status = request.Status;

                await _unitOfWork.OrderRepository.UpdateOrderAsync(orderId, order, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<OrderDto>.Ok(_mapper.Map<OrderDto>(order));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<bool>> DeleteOrderAsync(DeleteOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var order = await _unitOfWork.OrderRepository.GetOrderAsync(request.OrderId, cancellationToken);
                if (order is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<bool>.NotFound(key: request.OrderId, entityName: nameof(Domain.Models.Order));
                }

                await _unitOfWork.OrderRepository.DeleteOrderAsync(request.OrderId, cancellationToken);

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
    }
}
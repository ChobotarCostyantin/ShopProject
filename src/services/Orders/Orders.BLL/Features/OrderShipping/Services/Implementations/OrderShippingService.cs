using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Orders.BLL.Features.OrderShipping.DTOs.Requests;
using Orders.BLL.Features.OrderShipping.DTOs.Responces;
using Orders.BLL.Features.OrderShipping.Services.Interfaces;
using Orders.DAL.Repositories.UOW.Interfaces;
using Shared.ErrorHandling;
using Shared.Exceptions;

namespace Orders.BLL.Features.OrderShipping.Services.Implementations
{
    public class OrderShippingService : IOrderShippingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderShippingService> _logger;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateOrderShippingRequest> _createOrderShippingRequestValidator;
        private readonly IValidator<UpdateOrderShippingRequest> _updateOrderShippingRequestValidator;

        public OrderShippingService(
            IUnitOfWork unitOfWork,
            ILogger<OrderShippingService> logger,
            IMapper mapper,
            IValidator<CreateOrderShippingRequest> createOrderShippingRequestValidator,
            IValidator<UpdateOrderShippingRequest> updateOrderShippingRequestValidator
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _createOrderShippingRequestValidator = createOrderShippingRequestValidator;
            _updateOrderShippingRequestValidator = updateOrderShippingRequestValidator;
        }

        public async Task<Result<OrderShippingDto?>> GetOrderShippingByIdAsync(Guid shippingId, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderShipping = await _unitOfWork.OrderShippingRepository.GetOrderShippingAsync(shippingId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return orderShipping is null
                    ? Result<OrderShippingDto?>.NotFound(key: shippingId, entityName: nameof(Domain.Models.OrderShipping))
                    : Result<OrderShippingDto?>.Ok(_mapper.Map<OrderShippingDto>(orderShipping));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<OrderShippingDto?>> GetOrderShippingByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderShipping = await _unitOfWork.OrderShippingRepository.GetOrderShippingByOrderIdAsync(orderId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return orderShipping is null
                    ? Result<OrderShippingDto?>.NotFound(alternativeMessage: $"Order shipping for order with id of {orderId} was not found")
                    : Result<OrderShippingDto?>.Ok(_mapper.Map<OrderShippingDto>(orderShipping));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<OrderShippingDto>> CreateOrderShippingAsync(CreateOrderShippingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _createOrderShippingRequestValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result<OrderShippingDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
                }

                await _unitOfWork.BeginTransactionAsync();

                var order = await _unitOfWork.OrderRepository.GetOrderAsync(request.OrderId, cancellationToken);
                if (order is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<OrderShippingDto>.NotFound(key: request.OrderId, entityName: nameof(Domain.Models.Order));
                }

                var orderShipping = _mapper.Map<Domain.Models.OrderShipping>(request);
                orderShipping.ShippingId = Guid.CreateVersion7();

                await _unitOfWork.OrderShippingRepository.CreateOrderShippingAsync(orderShipping, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<OrderShippingDto>.Created($"/api/order-shipping/{orderShipping.ShippingId}", _mapper.Map<OrderShippingDto>(orderShipping));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<OrderShippingDto>> UpdateOrderShippingAsync(Guid shippingId, UpdateOrderShippingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _updateOrderShippingRequestValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result<OrderShippingDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
                }

                await _unitOfWork.BeginTransactionAsync();
                
                var orderShipping = await _unitOfWork.OrderShippingRepository.GetOrderShippingAsync(shippingId, cancellationToken);
                if (orderShipping is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<OrderShippingDto>.NotFound(key: shippingId, entityName: nameof(Domain.Models.OrderShipping));
                }

                orderShipping.AddressLine = request.AddressLine;
                orderShipping.City = request.City;
                orderShipping.PostalCode = request.PostalCode;

                await _unitOfWork.OrderShippingRepository.UpdateOrderShippingAsync(shippingId, orderShipping, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<OrderShippingDto>.Ok(_mapper.Map<OrderShippingDto>(orderShipping));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<bool>> DeleteOrderShippingAsync(DeleteOrderShippingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderShipping = await _unitOfWork.OrderShippingRepository.GetOrderShippingAsync(request.ShippingId, cancellationToken);
                if (orderShipping is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<bool>.NotFound(key: request.ShippingId, entityName: nameof(Domain.Models.OrderShipping));
                }

                await _unitOfWork.OrderShippingRepository.DeleteOrderShippingAsync(request.ShippingId, cancellationToken);

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
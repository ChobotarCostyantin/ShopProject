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

        private readonly IValidator<GetOrderShippingByIdRequest> _getOrderShippingByIdRequestValidator;
        private readonly IValidator<GetOrderShippingByOrderIdRequest> _getOrderShippingByOrderIdRequestValidator;
        private readonly IValidator<CreateOrderShippingRequest> _createOrderShippingRequestValidator;
        private readonly IValidator<UpdateOrderShippingRequest> _updateOrderShippingRequestValidator;
        private readonly IValidator<DeleteOrderShippingRequest> _deleteOrderShippingRequestValidator;

        public OrderShippingService(
            IUnitOfWork unitOfWork,
            ILogger<OrderShippingService> logger,
            IMapper mapper,
            IValidator<GetOrderShippingByIdRequest> getOrderShippingByIdRequestValidator,
            IValidator<GetOrderShippingByOrderIdRequest> getOrderShippingByOrderIdRequestValidator,
            IValidator<CreateOrderShippingRequest> createOrderShippingRequestValidator,
            IValidator<UpdateOrderShippingRequest> updateOrderShippingRequestValidator,
            IValidator<DeleteOrderShippingRequest> deleteOrderShippingRequestValidator
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _getOrderShippingByIdRequestValidator = getOrderShippingByIdRequestValidator;
            _getOrderShippingByOrderIdRequestValidator = getOrderShippingByOrderIdRequestValidator;
            _createOrderShippingRequestValidator = createOrderShippingRequestValidator;
            _updateOrderShippingRequestValidator = updateOrderShippingRequestValidator;
            _deleteOrderShippingRequestValidator = deleteOrderShippingRequestValidator;
        }

        public async Task<Result<OrderShippingDto?>> GetOrderShippingByIdAsync(GetOrderShippingByIdRequest request, CancellationToken cancellationToken)
        {
            await _getOrderShippingByIdRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderShipping = await _unitOfWork.OrderShippingRepository.GetOrderShippingAsync(request.ShippingId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return orderShipping is null
                    ? Result<OrderShippingDto?>.NotFound(key: request.ShippingId, entityName: nameof(Domain.Models.OrderShipping))
                    : Result<OrderShippingDto?>.Ok(_mapper.Map<OrderShippingDto>(orderShipping));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<OrderShippingDto?>> GetOrderShippingByOrderIdAsync(GetOrderShippingByOrderIdRequest request, CancellationToken cancellationToken)
        {
            await _getOrderShippingByOrderIdRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var orderShipping = await _unitOfWork.OrderShippingRepository.GetOrderShippingByOrderIdAsync(request.OrderId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return orderShipping is null
                    ? Result<OrderShippingDto?>.NotFound(alternativeMessage: $"Order shipping for order with id of {request.OrderId} was not found")
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
            await _createOrderShippingRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

            try
            {
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
            await _updateOrderShippingRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

            try
            {
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
            await _deleteOrderShippingRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

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
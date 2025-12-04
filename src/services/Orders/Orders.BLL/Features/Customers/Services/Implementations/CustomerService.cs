using System.Data.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Orders.BLL.Features.Customers.DTOs.Requests;
using Orders.BLL.Features.Customers.DTOs.Responses;
using Orders.BLL.Features.Customers.Services.interfaces;
using Orders.DAL.Repositories.UOW.Interfaces;
using Orders.Domain.Models;
using Shared.DTOs;
using Shared.ErrorHandling;
using Shared.Exceptions;

namespace Orders.BLL.Features.Customers.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerService> _logger;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateCustomerRequest> _createCustomerRequestValidator;
        private readonly IValidator<UpdateCustomerRequest> _updateCustomerRequestValidator;
        public CustomerService(
            IUnitOfWork unitOfWork,
            ILogger<CustomerService> logger,
            IMapper mapper,
            IValidator<CreateCustomerRequest> createCustomerRequestValidator,
            IValidator<UpdateCustomerRequest> updateCustomerRequestValidator
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _createCustomerRequestValidator = createCustomerRequestValidator;
            _updateCustomerRequestValidator = updateCustomerRequestValidator;
        }

        public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _createCustomerRequestValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result<CustomerDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
                }

                await _unitOfWork.BeginTransactionAsync();

                var customer = _mapper.Map<Customer>(request);
                customer.CustomerId = Guid.CreateVersion7();

                await _unitOfWork.CustomerRepository.CreateCustomerAsync(customer, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<CustomerDto>.Created($"/api/customers/{customer.CustomerId}", _mapper.Map<CustomerDto>(customer));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<CustomerDto>> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var customer = await _unitOfWork.CustomerRepository.GetCustomerAsync(customerId, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return customer is null
                    ? Result<CustomerDto>.NotFound(key: customerId, entityName: nameof(Customer))
                    : Result<CustomerDto>.Ok(_mapper.Map<CustomerDto>(customer));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<CustomerDto>> UpdateCustomerAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _updateCustomerRequestValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result<CustomerDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
                }

                await _unitOfWork.BeginTransactionAsync();

                var customer = await _unitOfWork.CustomerRepository.GetCustomerAsync(customerId, cancellationToken);
                if (customer is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<CustomerDto>.NotFound(key: customerId, entityName: nameof(Customer));
                }

                customer.FullName = request.FullName;

                await _unitOfWork.CustomerRepository.UpdateCustomerAsync(customerId, customer, cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<CustomerDto>.Ok(_mapper.Map<CustomerDto>(customer));
            }
            catch (DbException e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(e, "Error occured during database operation");
                throw e.ToInfrastructureException();
            }
        }

        public async Task<Result<bool>> DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var customer = await _unitOfWork.CustomerRepository.GetCustomerAsync(customerId, cancellationToken);
                if (customer is null)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return Result<bool>.NotFound(key: customerId, entityName: nameof(Customer));
                }

                await _unitOfWork.CustomerRepository.DeleteCustomerAsync(customerId, cancellationToken);

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

        public async Task<Result<PaginationResult<CustomerDto>>> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var customers = await _unitOfWork.CustomerRepository.GetCustomersAsync(request.PageSize, request.PageNumber, cancellationToken);
                var totalCount = await _unitOfWork.CustomerRepository.CountAllAsync(cancellationToken);

                await _unitOfWork.CommitTransactionAsync();

                return Result<PaginationResult<CustomerDto>>.Ok(new PaginationResult<CustomerDto>(
                    customers.Select(x => _mapper.Map<CustomerDto>(x)).ToArray(),
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
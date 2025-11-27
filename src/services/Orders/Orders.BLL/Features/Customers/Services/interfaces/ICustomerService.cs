using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.BLL.Features.Customers.DTOs.Requests;
using Orders.BLL.Features.Customers.DTOs.Responses;
using Shared.DTOs;
using Shared.ErrorHandling;

namespace Orders.BLL.Features.Customers.Services.interfaces
{
    public interface ICustomerService
    {
        Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken);
        Task<Result<CustomerDto>> GetCustomerByIdAsync(GetCustomerByIdRequest request, CancellationToken cancellationToken);
        Task<Result<CustomerDto>> UpdateCustomerAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteCustomerAsync(DeleteCustomerRequest request, CancellationToken cancellationToken);
        Task<Result<PaginationResult<CustomerDto>>> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellationToken);
    }
}
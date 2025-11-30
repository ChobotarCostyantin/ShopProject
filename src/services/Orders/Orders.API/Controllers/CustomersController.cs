using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orders.BLL.Features.Customers.DTOs.Requests;
using Orders.BLL.Features.Customers.DTOs.Responses;
using Orders.BLL.Features.Customers.Services.interfaces;
using Shared.DTOs;

namespace Orders.API.Controllers
{   
    [Route("api/[controller]")]
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{customerId:guid}")]
        [ProducesResponseType(typeof (CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return (await _customerService.GetCustomerByIdAsync(new GetCustomerByIdRequest(customerId), cancellationToken)).ToApiResponse();
        }

        [HttpGet]
        [ProducesResponseType(typeof (PaginationResult<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromQuery] GetCustomersRequest request, CancellationToken cancellationToken)
        {
            return (await _customerService.GetCustomersAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPost]
        [ProducesResponseType(typeof (CustomerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            return (await _customerService.CreateCustomerAsync(request, cancellationToken)).ToApiResponse();
        }

        [HttpPut("{customerId:guid}")]
        [ProducesResponseType(typeof (CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            return (await _customerService.UpdateCustomerAsync(customerId, request, cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{customerId:guid}")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return (await _customerService.DeleteCustomerAsync(new DeleteCustomerRequest(customerId), cancellationToken)).ToApiResponse();
        }
    }
}
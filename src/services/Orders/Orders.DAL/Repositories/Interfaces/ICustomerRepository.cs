using Orders.Domain.Models;

namespace Orders.DAL.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken);
        Task<List<Customer>> GetCustomersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
        Task<Customer> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> UpdateCustomerAsync(Guid customerId, Customer customer, CancellationToken cancellationToken);
        Task<bool> DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken);
        Task<long> CountAllAsync(CancellationToken cancellationToken);
    }
}
using Repository.Models;
using System.Linq.Expressions;

namespace Repository.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> FindAsync(Expression<Func<Customer, bool>> predicate);
        Task<bool> CreateAsync(Customer customer);
        Task<bool> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(Customer customer);
    }
}

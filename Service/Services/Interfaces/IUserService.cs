using Repository.Models;
using Repository.Dtos;
using System.Linq.Expressions;

namespace Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<Customer?> Login(string email, string password);
        Task<Customer?> GetCustomerById(int id);
        Task<Customer?> GetCustomer(Expression<Func<Customer, bool>> predicate);
        Task<bool> Register(CustomerDto dto);
        Task<bool> UpdateCustomer(Customer customer);
    }
}

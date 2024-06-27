using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Repository.Repositories.IRepositories;
using System.Linq.Expressions;

namespace Repository.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FuminiHotelManagementContext _context;

        public CustomerRepository(FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Customer customer)
        {
            _context.Customers.Remove(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Customer?> FindAsync(Expression<Func<Customer, bool>> predicate)
        {
            return await _context.Customers.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.EmailAddress.Equals(customer.EmailAddress));
            if (existingCustomer != null)
            {
                existingCustomer.CustomerFullName = customer.CustomerFullName ?? existingCustomer.CustomerFullName;
                existingCustomer.Telephone = customer.Telephone ?? existingCustomer.Telephone;
                existingCustomer.EmailAddress = customer.EmailAddress ?? existingCustomer.EmailAddress;
                existingCustomer.CustomerBirthday = customer.CustomerBirthday ?? existingCustomer.CustomerBirthday;
                existingCustomer.CustomerStatus = customer.CustomerStatus ?? existingCustomer.CustomerStatus;
                existingCustomer.Password = customer.Password ?? existingCustomer.Password;

                _context.Customers.Update(existingCustomer);
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}

using Repository.Dtos;
using Repository.Models;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using System.Linq.Expressions;

namespace Service.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailQueue _emailQueue;

        public UserService(ICustomerRepository customerRepository, IEmailQueue emailQueue)
        {
            _customerRepository = customerRepository;
            _emailQueue = emailQueue;
        }

        public async Task<Customer?> GetCustomer(Expression<Func<Customer, bool>> predicate)
        {
            return await _customerRepository.FindAsync(predicate);
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            var customer = await _customerRepository.FindAsync(c => c.CustomerId == id);

            if (customer == null) return null;

            return customer;
        }

        public async Task<Customer?> Login(string email, string password)
        {
            var customer = await _customerRepository.FindAsync(c => c.EmailAddress.Equals(email));

            if (customer == null) return null;

            var isMatch = customer.Password?.Equals(password) ?? false;

            if (isMatch == false) return null;

            return customer;
        }

        public async Task<bool> Register(CustomerDto dto)
        {
            var existCustomer = await _customerRepository.FindAsync(c => c.EmailAddress.Equals(dto.Email));

            if (existCustomer != null) return false;

            var newCustomer = new Customer
            {
                CustomerFullName = dto.CustomerFullName,
                EmailAddress = dto.Email,
                Password = dto.Password,
                Telephone = dto.Telephone,
                CustomerBirthday = DateOnly.FromDateTime(dto.CustomerBirthday),
                CustomerStatus = 0,
            };

            var result = await _customerRepository.CreateAsync(newCustomer);

            if (result)
            {
                await _emailQueue.EnqueueEmailAsync(newCustomer.EmailAddress);
            }

            return result;
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            var result = await _customerRepository.UpdateAsync(customer);
            return result;
        }
    }
}

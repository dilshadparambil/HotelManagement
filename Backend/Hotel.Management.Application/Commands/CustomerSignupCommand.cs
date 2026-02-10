using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Commands
{
    public class CustomerSignupCommand : IRequest<Unit> 
    {
        public CustomerSignupDTO Dto { get; set; }
    }


    public class CustomerSignupCommandHandler : IRequestHandler<CustomerSignupCommand, Unit>
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerSignupCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Unit> Handle(CustomerSignupCommand request, CancellationToken cancellationToken)
        {

            var exists = await _customerRepository.CustomerExistsAsync(request.Dto.Username, request.Dto.Email);

            if (exists)
            {
                throw new Exception("Username or Email already exists.");
            }

            var customer = new Customer
            {
                FullName = request.Dto.FullName,
                Email = request.Dto.Email,
                PhoneNumber = request.Dto.PhoneNumber,
                IdProofNumber = request.Dto.IdProofNumber,
                Username = request.Dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Dto.Password)
        };
            await _customerRepository.AddCustomerAsync(customer);
            return Unit.Value;
        }
    }
}
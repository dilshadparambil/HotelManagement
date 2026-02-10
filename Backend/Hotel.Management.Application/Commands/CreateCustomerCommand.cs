using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerResponseDTO>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdProofNumber { get; set; }
    }
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerResponseDTO>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository CustomerRepository)
        {
            _customerRepository = CustomerRepository;
        }

        public async Task<CustomerResponseDTO> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                IdProofNumber = request.IdProofNumber
            };

            var result = await _customerRepository.AddCustomerAsync(customer);

            return new CustomerResponseDTO
            {
                Id = result.Id,
                FullName = result.FullName,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                IdProofNumber = customer.IdProofNumber

            };
        }
    }
}
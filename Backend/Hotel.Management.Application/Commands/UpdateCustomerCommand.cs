using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class UpdateCustomerCommand : IRequest<CustomerResponseDTO>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string IdProofNumber { get; set; }
    }
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerResponseDTO>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository CustomerRepository)
        {
            _customerRepository = CustomerRepository;
        }

        public async Task<CustomerResponseDTO> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(request.Id);
            if (customer == null)
                return null;

            customer.FullName = request.FullName ?? customer.FullName;
            customer.PhoneNumber = request.PhoneNumber ?? customer.PhoneNumber;
            customer.IdProofNumber = request.IdProofNumber ?? customer.IdProofNumber;

            await _customerRepository.UpdateCustomerAsync(customer);
            return new CustomerResponseDTO
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                IdProofNumber = customer.IdProofNumber
            };
        }
    }
}
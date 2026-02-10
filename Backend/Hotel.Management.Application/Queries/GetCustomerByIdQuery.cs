using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerResponseDTO>
    {
        public int Id { get; set; }
    }
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery,CustomerResponseDTO>
    {
        private readonly ICustomerRepository _customerRepository;


        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponseDTO> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await  _customerRepository.GetCustomerByIdAsync(request.Id);
            if (customer == null)
            {
                return null;
            }
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
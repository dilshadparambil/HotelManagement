using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetAllCustomersQuery : IRequest<List<CustomerResponseDTO>>
    {
    }
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerResponseDTO>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersQueryHandler(ICustomerRepository CustomerRepository)
        {
            _customerRepository = CustomerRepository;
        }

        public async Task<List<CustomerResponseDTO>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var Customers = await _customerRepository.GetAllCustomersAsync();
            var dtoList = Customers.Select(c => new CustomerResponseDTO
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                IdProofNumber = c.IdProofNumber
            })
            .ToList();
            return dtoList;
        }
    }
}
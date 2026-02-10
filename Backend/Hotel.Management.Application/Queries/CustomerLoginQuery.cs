using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Application.Services;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{

    public class CustomerLoginQuery : IRequest<AuthResponseDTO>
    {
        public LoginDTO userloginDTO { get; set; }
    }



    public class CustomerLoginQueryHandler : IRequestHandler<CustomerLoginQuery, AuthResponseDTO>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITokenService _tokenService;

        public CustomerLoginQueryHandler(ICustomerRepository customerRepository, ITokenService tokenService)
        {
            _customerRepository = customerRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO> Handle(CustomerLoginQuery request, CancellationToken cancellationToken)
        {
            var customer = (await _customerRepository.GetAllCustomersAsync())
                .FirstOrDefault(c => c.Username == request.userloginDTO.Username);


            if (customer == null || !BCrypt.Net.BCrypt.Verify(request.userloginDTO.Password, customer.Password))
            {
                return null; 
            }

            var accessToken = _tokenService.CreateAccessToken(customer);
            var refreshToken = _tokenService.CreateRefreshToken();

            
            customer.RefreshToken = refreshToken;
            customer.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await _customerRepository.UpdateCustomerAsync(customer);

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
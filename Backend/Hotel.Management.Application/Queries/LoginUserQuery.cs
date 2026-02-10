using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Application.Services;
using Hotel.Management.Domain.Interfaces;


namespace Hotel.Management.Application.Queries
{

    public class LoginUserQuery : IRequest<AuthResponseDTO>
    {
        public LoginDTO adminLoginDto { get; set; }
    }


    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthResponseDTO>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITokenService _tokenService; 

        public LoginUserQueryHandler(IEmployeeRepository employeeRepository, ITokenService tokenService)
        {
            _employeeRepository = employeeRepository;
            _tokenService = tokenService;
        }


        public async Task<AuthResponseDTO> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {


            var user = (await _employeeRepository.GetAllEmployeesAsync())
                .FirstOrDefault(u => u.Username == request.adminLoginDto.Username);



            if (user == null || user.Password != request.adminLoginDto.Password)
            {
                return null;
            }


            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();


            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await _employeeRepository.UpdateEmployeeAsync(user);


            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
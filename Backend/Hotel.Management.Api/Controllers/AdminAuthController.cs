using MediatR;
using Microsoft.AspNetCore.Mvc;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Application.Queries;
using Hotel.Management.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Hotel.Management.Infrastructure.Implementations;
using Hotel.Management.Domain.Interfaces;

namespace HotelMangement.Controllers
{
    [Route("api/admin-auth")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITokenService _tokenService;


        public AdminAuthController(IMediator mediator, IEmployeeRepository employeeRepository, ITokenService tokenService)
        {
            _mediator = mediator;
            _employeeRepository = employeeRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var query = new LoginUserQuery { adminLoginDto = loginDto };
            var authResponse = await _mediator.Send(query);

            if (authResponse == null)
            {
                return Unauthorized("Invalid username or password");
            }

            Response.Cookies.Delete("accessToken", new CookieOptions { Path = "/" });
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/customer-auth/refresh" });

            Response.Cookies.Append("accessToken", authResponse.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(15),
                Path = "/"
            });

            Response.Cookies.Append("refreshToken", authResponse.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(7),
                Path = "/api/admin-auth/refresh"
            });

            return Ok(new { Message = "Login Successful" });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized("No refresh token");
            }

            var user = (await _employeeRepository.GetAllEmployeesAsync())
   .FirstOrDefault(e => e.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.Now)
            {
                return Unauthorized("Invalid refresh token");
            }

            var newAccessToken = _tokenService.CreateAccessToken(user);

            Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(15),
                Path = "/"
            });

            var newRefreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await _employeeRepository.UpdateEmployeeAsync(user);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(7),
                Path = "/api/admin-auth/refresh"
            });

            return Ok(new { Message = "Token refreshed" });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken", new CookieOptions { Path = "/" });
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/admin-auth/refresh" });
            return Ok(new { Message = "Logged out" });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role != "Admin")
            {
                return Unauthorized("Not an admin");
            }

            return Ok(new
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Name = User.FindFirstValue(ClaimTypes.Name),
                Role = role
            });
        }
    }
}
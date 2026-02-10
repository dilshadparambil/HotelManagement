using MediatR;
using Microsoft.AspNetCore.Mvc;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Application.Commands;
using Hotel.Management.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Hotel.Management.Application.Services;
using Hotel.Management.Infrastructure.Implementations;
using Hotel.Management.Domain.Interfaces;

namespace HotelMangement.Controllers
{
    [Route("api/customer-auth")]
    [ApiController]
    public class CustomerAuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITokenService _tokenService;

        public CustomerAuthController(IMediator mediator, ICustomerRepository customerRepository, ITokenService tokenService)
        {
            _mediator = mediator;
            _customerRepository = customerRepository;
            _tokenService = tokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] CustomerSignupDTO dto)
        {
            try
            {
                await _mediator.Send(new CustomerSignupCommand { Dto = dto });
                return Ok(new { message = "Signup successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var query = new CustomerLoginQuery { userloginDTO = loginDto };
            var authResponse = await _mediator.Send(query);

            if (authResponse == null)
            {
                return Unauthorized("Invalid username or password");
            }

            
            
            Response.Cookies.Delete("accessToken", new CookieOptions { Path = "/" });
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/admin-auth/refresh" });
            
            
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
                Path = "/api/customer-auth/refresh" 
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

            var user = (await _customerRepository.GetAllCustomersAsync())
                .FirstOrDefault(c => c.RefreshToken == refreshToken);

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
            await _customerRepository.UpdateCustomerAsync(user);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddDays(7),
                Path = "/api/customer-auth/refresh"
            });

            return Ok(new { Message = "Token refreshed" });
        }

        [HttpGet("me")]
        [Authorize] 
        public IActionResult Me()
        {
            var userType = User.FindFirstValue("UserType");

            
            if (userType != "Customer")
            {
                return Unauthorized("Not a customer");
            }

            return Ok(new
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Name = User.FindFirstValue(ClaimTypes.Name),
                Role = "Customer"
            });
        }

        
        [HttpPost("logout")]
        public IActionResult Logout()
        {
        
            Response.Cookies.Delete("accessToken", new CookieOptions { Path = "/" });
            Response.Cookies.Delete("refreshToken", new CookieOptions { Path = "/api/customer-auth/refresh" });
            return Ok(new { Message = "Logged out" });
        }
    }
}
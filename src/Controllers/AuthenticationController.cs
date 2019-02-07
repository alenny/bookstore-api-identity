using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStore.Api.Identity.Models;
using BookStore.Api.Identity.Requests;
using BookStore.Api.Identity.Responses;
using BookStore.Api.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.Api.Identity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly AppSettings _appSettings;

        public AuthenticationController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> Post(AuthenticationRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userService.GetUserAsync(req.UserName, req.Password);
            if (user == null)
            {
                return BadRequest(new Error { ErrorMessage = "UserName or Password is incorrect" });
            }
            return Ok(PostAuth(user));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Post(RegisterRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userService.GetUserAsync(req.UserName);
            if (user != null)
            {
                return BadRequest(new Error { ErrorMessage = "User exists" });
            }
            user = await _userService.CreateUser(req.UserName, req.Password);
            return Ok(PostAuth(user));
        }

        private AuthenticationResponse PostAuth(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(descriptor);
            var tokenText = tokenHandler.WriteToken(token);
            return new AuthenticationResponse
            {
                Id = user.Id,
                Name = user.Name,
                JwtToken = tokenText
            };
        }
    }
}

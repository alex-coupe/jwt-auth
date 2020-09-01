using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtAuth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private AuthenticationContext _context;
       public AuthenticationController(IConfiguration config, AuthenticationContext context)
       {
            _config = config;
            _context = context;
       }

       [AllowAnonymous]
       
       [HttpGet]
       [Route("Login")]
        public IActionResult Login([FromBody] UserDTO credentials)
       {
            var user = AuthenticateUser(credentials);

            if (user != null)
            {
                var token = GenerateJSONWebToken(user);
                return Ok(new { token_type="Bearer", token = new JwtSecurityTokenHandler().WriteToken(token), expires=token.ValidTo });
            }
            return Unauthorized("Username or Password Incorrect");
       }       

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO credentials)
        {
            var user = new User
            {
                Username = credentials.Username,
                Password = CryptoHelper.Crypto.HashPassword(credentials.Password)
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Register", null, user.Id);
        }

        private JwtSecurityToken GenerateJSONWebToken(UserDTO credentials)
        {
            var secruityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(secruityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("DateCreated", credentials.DateCreated.ToString("yyyy-MM-dd"))
            };

            var token = new JwtSecurityToken(null,
                null,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: signingCredentials);

            return token;
        }

        private UserDTO AuthenticateUser(UserDTO login)
        {
            var hash = CryptoHelper.Crypto.HashPassword(login.Password);
            var user = _context.Users.Where(x => x.Username == login.Username).FirstOrDefault();
            
            if (user != null && CryptoHelper.Crypto.VerifyHashedPassword(user.Password, login.Password))
            {
                return new UserDTO
                {
                    Username = user.Username,
                    Password = hash,
                    DateCreated = user.DateCreated
                };
            }
            return null;
        }
    }
}

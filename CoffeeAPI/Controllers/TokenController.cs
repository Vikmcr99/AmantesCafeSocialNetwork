using Core.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeAPI.Controllers
{
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly CoffeeDbContext _context;
        public TokenController(IConfiguration configuration, CoffeeDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Post(User userInfo)
        {
            if (userInfo != null && userInfo.UserName != null && userInfo.Password != null)
            {
                var user = await GetUser(userInfo.UserName, userInfo.Password);
                if (user != null)
                {
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                    new Claim("UserId",user.UserId.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Password", user.Password)

                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.Now.AddMinutes(20),
                            signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }


            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<User> GetUser(string userName, string pass)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == pass);
        }
    }
}

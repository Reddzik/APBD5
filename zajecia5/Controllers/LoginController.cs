using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using zajecia5.DOTs.Requests;

namespace zajecia5.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration Configuration;
        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpPost]
        public IActionResult Login(LoginRequest req)
        {
            var claims = new[]
{
                new Claim(type: ClaimTypes.NameIdentifier,"1"),
                new Claim(ClaimTypes.Name,"StudentName"),
                new Claim(ClaimTypes.Role,"admin"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: "Gakko",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                ) ;


            return Ok(new {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken=Guid.NewGuid() 
            }) ;
        }
    }
}
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
using zajecia5.Services;

namespace zajecia5.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration Configuration;
        private IStudentDbService _service;
        public LoginController(IConfiguration configuration, IStudentDbService service)
        {
            Configuration = configuration;
            _service = service;
        }
        [HttpPost]
        public IActionResult Login(LoginRequest req)
        {
            if (!_service.CheckCredential(req.username, req.password))
                return StatusCode(403);

            var claims = new[]
{
                new Claim(type: ClaimTypes.NameIdentifier,req.username),
                new Claim(ClaimTypes.Role,"employee"),
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
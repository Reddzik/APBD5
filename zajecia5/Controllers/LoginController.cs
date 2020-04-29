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

            var user = _service.GetLoggedStudent(req.username, req.password);

            if (user == null) return StatusCode(403);

            Console.WriteLine(user.FirstName, user.IndexNumber);

            var claims = new[]
            {
                new Claim(type: ClaimTypes.NameIdentifier,user.IndexNumber),
                new Claim(ClaimTypes.Role,"employee"),
                new Claim(ClaimTypes.Name, user.FirstName),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: "Gakko",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: creds
                ) ;
            var refreshToken = Guid.NewGuid();
            _service.AddRefreshTokenToUser(refreshToken.ToString(), user.IndexNumber);
            return Ok(new {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            }) ;
        }
        [HttpPost("refresh-token/{refToken}")]
        public IActionResult RefreshTokenLogin(string refToken)
        {
            var user = _service.GetUserByRefreshToken(refToken);

            if (user == null) return StatusCode(403);

            var claims = new[]
            {
                new Claim(type: ClaimTypes.NameIdentifier,user.IndexNumber),
                new Claim(ClaimTypes.Role,"employee"),
                new Claim(ClaimTypes.Name, user.FirstName),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
                (
                    issuer: "Gakko",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: creds
                );
            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = refToken
            });
        }
    }
}
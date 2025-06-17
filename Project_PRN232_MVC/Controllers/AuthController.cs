using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_PRN232_MVC.ModelDTO;

namespace Project_PRN232_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Prn222BeverageWebsiteProjectContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, Prn222BeverageWebsiteProjectContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // POST: api/SystemAccounts/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] AccountRequestDTO loginDTO)
        {
            try
            {
                var account = _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Email == loginDTO.Email && u.Password == loginDTO.Password);

                if (account == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                //IConfiguration configuration = new ConfigurationBuilder()
                //    .SetBasePath(Directory.GetCurrentDirectory())
                //    .AddJsonFile("appsettings.json", true, true)
                //    .Build();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim("Role", account.Role.RoleName.ToString()),
                    new Claim("AccountId", account.UserId.ToString())
                };
                var secret = _configuration["JWT:SecretKey"];
                if (secret == null)
                {
                    return BadRequest("JWT:SecretKey not found in configuration.");
                }
                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var signCredential = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

                var preparedToken = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(16),
                    signingCredentials: signCredential
                );

                var generatedToken = new JwtSecurityTokenHandler().WriteToken(preparedToken);
                var role = account.Role.RoleName.ToString();
                var accountId = account.UserId.ToString();

                return Ok(new AccountResponseDTO
                {
                    Role = role,
                    Token = generatedToken,
                    AccountId = accountId
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

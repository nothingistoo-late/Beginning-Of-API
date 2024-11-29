using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Final4.Data;
using Final4.DTO.Account;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Final4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _dbContext;
        public AccountController(ApplicationDBContext DBContext, IConfiguration configuration)
        {
            _dbContext = DBContext;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUserAccount(RegisterUserAccount obj)
        {
            var listUser = _dbContext.Accounts.ToList();
            if (listUser.Any(x => x.Email == obj.Email))
                return BadRequest("Exited Account");
            var AccountEntity = new Account()
            {
                Name = obj.Name,
                Email = obj.Email,
                Password = obj.Password,
                RoleID = "User"
            };
            _dbContext.Accounts.Add(AccountEntity);
            _dbContext.SaveChanges();
            return Ok(obj);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginUserAccount obj)
        {
            var listUser = _dbContext.Accounts.ToList();
            var checkAccountExits = listUser.FirstOrDefault(x => x.Email == obj.Email && x.Password == obj.Password);

            if (checkAccountExits != null)
            {
                // Lấy thông tin từ appsettings
                var jwtConfig = _configuration.GetSection("JwtConfig");
                var issuer = jwtConfig["Issuer"];
                var audience = jwtConfig["Audience"];
                var key = jwtConfig["Key"];
                var expirationMinutes = int.Parse(jwtConfig["TokenValidityMins"]);

                // Tạo các claim (ví dụ Role)
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, checkAccountExits.Email),
                    new Claim(ClaimTypes.Role, checkAccountExits.RoleID)  // Thêm role vào claim
                };

                // Tạo token
                var keyByteArray = Encoding.UTF8.GetBytes(key);
                var securityKey = new SymmetricSecurityKey(keyByteArray);
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = expiration,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                // Trả về token và thời gian hết hạn
                var response = new
                {
                    Token = tokenHandler.WriteToken(token),
                    Expiration = expiration
                };

                return Ok(response);  // Trả về token và thời gian hết hạn
            }
            else
            {
                return Unauthorized("Invalid credentials.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        [Route("GetAllAccount")]
        public IActionResult GetAccount()
        {
            //return Ok(_dbContext.Accounts.Select(a=> new {a.Name, a.Email}) .ToList());
            return Ok(_dbContext.Accounts.ToList());
        }
    }
}

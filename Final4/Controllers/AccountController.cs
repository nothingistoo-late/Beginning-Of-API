using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Final4.Data;
using Final4.DTO.Account;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly EmailService _emailService;
        public AccountController(ApplicationDBContext DBContext, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = DBContext;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUserAccount(RegisterUserAccount obj)
        {
            var listUser = _dbContext.Accounts.ToList();
            if (listUser.Any(x => x.AccountEmail == obj.Email))
                return BadRequest("Exited Gmail, Please Prove Another Gmail Or Click The Forgot Password Button ");
            var AccountEntity = new Account()
            {
                AccountName = obj.Name,
                AccountEmail = obj.Email,
                AccountPassword = obj.Password,
                AccountRoleID = "User"
            };
            _dbContext.Accounts.Add(AccountEntity);
            _dbContext.SaveChanges();
            await _emailService.SendEmailAsync(obj.Email, "Created Account Successfully", "You Had Create A Account In Our Page, Your Passowrd Is '" + obj.Password + "'\n Please Do Not Provide This Password For Any One ");
            return Ok(obj);
        }

            [HttpPost]
            [Route("Login")]
            public async Task<IActionResult> Login(LoginUserAccount obj)
            {
                var listUser = await _dbContext.Accounts.ToListAsync();
                var checkAccountExits = listUser.FirstOrDefault(x => x.AccountEmail == obj.Email && x.AccountPassword == obj.Password);

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
                        new Claim(ClaimTypes.Email, checkAccountExits.AccountEmail),
                        new Claim(ClaimTypes.Role, checkAccountExits.AccountRoleID),  // Thêm role vào claim
                        new Claim("AccountId", checkAccountExits.AccountId.ToString()) // Thêm AccountId

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
        [HttpDelete]
        [Route("DeleteUserBy{id}")]
        public async Task<IActionResult> DeleteUserByID(int id)
        {
            var user = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);
            if (user == null)
                return BadRequest("InValid User");
            _dbContext.Accounts.Remove(user);
            await _dbContext.SaveChangesAsync();
            return Ok("Delete Succesfully!!");
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        [Route("GetAllAccount")]
        public async Task<IActionResult> GetAccount()
        {
            // Kiểm tra xem người dùng có vai trò 'Admin' không
            if (!User.IsInRole("Admin"))
                return Unauthorized("You are not authorized to access this resource.");
            else
                return Ok(await _dbContext.Accounts.ToListAsync());
        }
        [HttpPut]
        [Route("FogetPassword{email}")]
        public async Task<IActionResult> FogetPassword(string email, ResetPassword obj)
        {
            var listUser = await _dbContext.Accounts.ToListAsync();
            var checkAccountExits = listUser.FirstOrDefault(x => x.AccountEmail == email);
            if (checkAccountExits == null)
                return BadRequest("Invalid Email, Check Email And Try Again!!!");
            if (obj.NewPassword.Equals(obj.ConfirmPassword))
            {
                checkAccountExits.AccountPassword = obj.ConfirmPassword;
                await _dbContext.SaveChangesAsync();
                await _emailService.SendEmailAsync(email, "Your Password In Final4 Has Reset Succesfully", "Your Password Has Been Changed, Your New Password Is " + obj.ConfirmPassword+"\n Please Change Your Password if this is not done by you");
                return Ok("Updated Completed");
            }
            else return BadRequest("Confirm Password Doesnt Match New Password");
        }
    }
}

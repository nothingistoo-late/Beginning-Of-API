﻿using System.IdentityModel.Tokens.Jwt;
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
    //[Route("api/[controller]")]
    //[ApiController]
    //public class AccountController : ControllerBase
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly ApplicationDBContext _dbContext;
    //    private readonly EmailService _emailService;
    //    public AccountController(ApplicationDBContext DBContext, IConfiguration configuration, EmailService emailService)
    //    {
    //        _dbContext = DBContext;
    //        _configuration = configuration;
    //        _emailService = emailService;
    //    }

    //    [HttpPost]
    //    [Route("Register")]
    //    public async Task<IActionResult> RegisterUserAccount(List<RegisterUserAccount> regisaccount)
    //    {
    //        //var accountToAdd = new List<Account>();
    //        //foreach (RegisterUserAccount obj in regisaccount)
    //        //{
    //        //    if (_dbContext.Accounts.Any(x => x.AccountEmail == obj.Email))
    //        //        return BadRequest("Exited Gmail, Please Prove Another Gmail Or Click The Forgot Password Button ");
    //        //    var AccountEntity = new Account()
    //        //    {
    //        //        AccountName = obj.Name,
    //        //        AccountEmail = obj.Email,
    //        //        AccountPassword = obj.Password,
    //        //        AccountRoleID = "User"
    //        //    };
    //        //    accountToAdd.Add(AccountEntity);
    //        //    //_dbContext.Accounts.Add(AccountEntity);
    //        //    //_dbContext.SaveChanges();
    //        //    await _emailService.SendEmailAsync(obj.Email, "Created Account Successfully", "You Had Create A Account In Our Page, Your Passowrd Is '" + obj.Password + "'\n Please Do Not Provide This Password For Any One ");
    //        //}
    //        //await _dbContext.Accounts.AddRangeAsync(accountToAdd);
    //        //await _dbContext.SaveChangesAsync();
    //        //return Ok();
    //        var accountToAdd = new List<Account>();

    //        foreach (RegisterUserAccount obj in regisaccount)
    //        {
    //            // Kiểm tra email trong cơ sở dữ liệu mà không tải toàn bộ dữ liệu vào bộ nhớ
    //            if (_dbContext.Accounts.Any(x => x.AccountEmail == obj.Email))
    //                return BadRequest("Exited Gmail, Please Provide Another Gmail Or Click The Forgot Password Button");

    //            var AccountEntity = new Account()
    //            {
    //                AccountName = obj.Name,
    //                AccountEmail = obj.Email,
    //                AccountPassword = obj.Password,
    //                AccountRoleID = "User"
    //            };

    //            accountToAdd.Add(AccountEntity);
    //        }

    //        try
    //        {
    //            // Thêm tất cả tài khoản vào cơ sở dữ liệu trong một lần
    //            await _dbContext.Accounts.AddRangeAsync(accountToAdd);
    //            await _dbContext.SaveChangesAsync();

    //            // Gửi email sau khi đã thêm tất cả tài khoản thành công
    //            foreach (var obj in regisaccount)
    //            {
    //                var subject = "Account Created Successfully!";
    //                var body = $@"
    //                <p>Dear {obj.Email},</p>
    //                <p>We are excited to inform you that your account has been successfully created on our platform!</p>
    //                <p><strong>Your account details:</strong></p>
    //                <ul>
    //                <li><strong>Email:</strong> {obj.Email}</li>
    //                <li><strong>Password:</strong> <em>Your password has been securely stored. Please change it after your first login.</em></li>
    //                </ul>
    //                <p>If you have any questions or issues, feel free to contact our support team.</p>
    //                <p>Thank you for joining us!</p>
    //                <br>
    //                <p>Best regards,<br>Your Website Team</p>";
    //                await _emailService.SendEmailAsync(new List<string> { obj.Email }, subject, body);

    //            }
    //            return Ok();
    //        }
    //        catch (Exception ex)
    //        {
    //            // Bắt và xử lý lỗi nếu có
    //            return StatusCode(500, "Internal Server Error: " + ex.Message);
    //        }
    //    }

    //    [HttpPost]
    //    [Route("Login")]
    //    public async Task<IActionResult> Login(LoginUserAccount obj)
    //    {
    //        var checkAccountExits = _dbContext.Accounts.FirstOrDefault(x => x.AccountEmail == obj.Email && x.AccountPassword == obj.Password);

    //        if (checkAccountExits != null)
    //        {
    //            // Lấy thông tin từ appsettings
    //            var jwtConfig = _configuration.GetSection("JwtConfig");
    //            var issuer = jwtConfig["Issuer"];
    //            var audience = jwtConfig["Audience"];
    //            var key = jwtConfig["Key"];
    //            var expirationMinutes = int.Parse(jwtConfig["TokenValidityMins"]);

    //            // Tạo các claim (ví dụ Role)
    //            var claims = new[]
    //            {
    //                    new Claim(ClaimTypes.Email, checkAccountExits.AccountEmail),
    //                    new Claim(ClaimTypes.Role, checkAccountExits.AccountRoleID),  // Thêm role vào claim
    //                    new Claim("AccountId", checkAccountExits.AccountId.ToString()) // Thêm AccountId
    //                };

    //            // Tạo token
    //            var keyByteArray = Encoding.UTF8.GetBytes(key);
    //            var securityKey = new SymmetricSecurityKey(keyByteArray);
    //            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

    //            var tokenDescriptor = new SecurityTokenDescriptor
    //            {
    //                Subject = new ClaimsIdentity(claims),
    //                Expires = expiration,
    //                Issuer = issuer,
    //                Audience = audience,
    //                SigningCredentials = credentials
    //            };

    //            var tokenHandler = new JwtSecurityTokenHandler();
    //            var token = tokenHandler.CreateToken(tokenDescriptor);

    //            // Trả về token và thời gian hết hạn
    //            var response = new
    //            {
    //                Token = tokenHandler.WriteToken(token),
    //                Expiration = expiration
    //            };

    //            return Ok(response);  // Trả về token và thời gian hết hạn
    //        }
    //        else
    //        {
    //            return Unauthorized("Invalid credentials.");
    //        }
    //    }

    //    [Authorize(Policy = "AdminPolicy")]
    //    [HttpDelete]
    //    [Route("DeleteUserByid/{id}")]
    //    public async Task<IActionResult> DeleteUserByID(int id)
    //    {
    //        var user = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);
    //        if (user == null)
    //            return BadRequest("InValid User");
    //        _dbContext.Accounts.Remove(user);
    //        await _dbContext.SaveChangesAsync();
    //        return Ok("Delete Succesfully!!");
    //    }

    //    [Authorize(Policy = "AdminPolicy")]
    //    [HttpGet]
    //    [Route("GetAllAccount")]
    //    public async Task<IActionResult> GetAccount()
    //    {
    //        // Kiểm tra xem người dùng có vai trò 'Admin' không
    //        if (!User.IsInRole("Admin"))
    //            return Unauthorized("You are not authorized to access this resource.");
    //        else
    //            return Ok(await _dbContext.Accounts.ToListAsync());
    //    }
    //    [HttpPut]
    //    [Route("FogetPasswordByEmail/{email}")]
    //    public async Task<IActionResult> FogetPassword(string email, ResetPassword obj)
    //    {
    //        var checkAccountExits = _dbContext.Accounts.FirstOrDefault(x => x.AccountEmail == email);
    //        if (checkAccountExits == null)
    //            return BadRequest("Invalid Email, Check Email And Try Again!!!");
    //        if (obj.NewPassword.Equals(obj.ConfirmPassword))
    //        {
    //            checkAccountExits.AccountPassword = obj.ConfirmPassword;
    //            await _dbContext.SaveChangesAsync();
    //            var subject = "Your Password Has Been Successfully Reset";
    //            var body = $@"
    //            <p>Dear {email},</p>
    //            <p>We wanted to let you know that your password has been successfully reset.</p>
    //            <p><strong>New password:</strong> <em>{obj.ConfirmPassword}</em></p>
    //            <p>If you did not request this change, please reset your password immediately to ensure your account remains secure.</p>
    //            <p>If you encounter any issues, don't hesitate to contact our support team.</p>
    //            <br>
    //            <p>Best regards,<br>Your Website Team</p>";
    //            await _emailService.SendEmailAsync(new List<string> { email }, subject, body);

    //            return Ok("Updated Completed");
    //        }
    //        else return BadRequest("Confirm Password Doesnt Match New Password");
    //    }
    //    [Authorize(Policy = "AdminPolicy")]
    //    [HttpGet]
    //    [Route("GetUserByGmail")]
    //    public async Task<IActionResult> GetUserByGmail(string email)
    //    {
    //        var checkAccountExits = _dbContext.Accounts.FirstOrDefault(x => x.AccountEmail == email);
    //        if (checkAccountExits == null)
    //            return BadRequest("Invalid Email, Check Email And Try Again!!!");
    //        else
    //            return Ok(checkAccountExits);
    //    }

    //    [HttpPost]
    //    [Route("FastLogin")]
    //    public async Task<IActionResult> FastLogin()
    //    {
    //        var anonymousUserId = Guid.NewGuid().ToString();

    //        // Lấy thông tin từ appsettings
    //        var jwtConfig = _configuration.GetSection("JwtConfig");
    //        var issuer = jwtConfig["Issuer"];
    //        var audience = jwtConfig["Audience"];
    //        var key = jwtConfig["Key"];
    //        var expirationMinutes = int.Parse(jwtConfig["TokenValidityMins"]);

    //        // Tạo các claim (ví dụ Role)
    //        var claims = new[]
    //     {
    //        new Claim(JwtRegisteredClaimNames.Sub, anonymousUserId), // ID ẩn danh
    //        new Claim(ClaimTypes.Role, "Anonymous"), // Vai trò: Anonymous
    //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token ID
    //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()) // Thời gian tạo
    //    };

    //        // Tạo token
    //        var keyByteArray = Encoding.UTF8.GetBytes(key);
    //        var securityKey = new SymmetricSecurityKey(keyByteArray);
    //        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(claims),
    //            Expires = expiration,
    //            Issuer = issuer,
    //            Audience = audience,
    //            SigningCredentials = credentials
    //        };

    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var token = tokenHandler.CreateToken(tokenDescriptor);

    //        // Trả về token và thời gian hết hạn
    //        var response = new
    //        {
    //            Token = tokenHandler.WriteToken(token),
    //            Expiration = expiration
    //        };
    //        return Ok(response);  // Trả về token và thời gian hết hạn
    //    }
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAccount(List<RegisterUserAccount> regisaccount)
        {
            var result = await _accountRepository.RegisterUserAccounts(regisaccount);
            if (!result)
                return BadRequest("Exited Gmail, Please Provide Another Gmail Or Click The Forgot Password Button");
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserAccount obj)
        {
            var token = await _accountRepository.Login(obj);
            if (token == null)
                return Unauthorized("Invalid credentials.");
            return Ok(new { Token = token });
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("DeleteUserById/{id}")]
        public async Task<IActionResult> DeleteUserByID(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            var result = await _accountRepository.Delete(account);
            return result ? Ok("Delete Successfully!!") : BadRequest("Invalid User");
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAccount()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return Ok(accounts);
        }

        [HttpPut("FogetPasswordByEmail/{email}")]
        public async Task<IActionResult> ForgetPassword(string email, ResetPassword obj)
        {
            var result = await _accountRepository.ResetPassword(email, obj);
            return result ? Ok("Updated Completed") : BadRequest("Confirm Password Doesn't Match New Password");
        }

        [HttpGet("GetUserByGmail")]
        public async Task<IActionResult> GetUserByGmail(string email)
        {
            var user = await _accountRepository.GetUserByEmail(email);
            return user == null ? BadRequest("Invalid Email") : Ok(user);
        }

        [HttpPost("FastLogin")]
        public async Task<IActionResult> FastLogin()
        {
            var token = await _accountRepository.FastLogin();
            return Ok(new { Token = token });
        }
    }
}

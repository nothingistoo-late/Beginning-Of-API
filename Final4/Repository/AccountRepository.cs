using Final4.Data;
using Final4.DTO.Account;
using Final4.IRepository;
using Final4.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AccountRepository 
{
    private readonly ApplicationDBContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly EmailService _emailService;

    public AccountRepository(ApplicationDBContext dbContext, IConfiguration configuration, EmailService emailService)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<bool> RegisterUserAccounts(List<RegisterUserAccount> accounts)
    {
        var accountToAdd = new List<Account>();

        foreach (var obj in accounts)
        {
            if (_dbContext.Accounts.Any(x => x.AccountEmail == obj.Email))
                return false;

            var AccountEntity = new Account()
            {
                AccountName = obj.Name,
                AccountEmail = obj.Email,
                AccountPassword = obj.Password,
                AccountRoleID = "User"
            };
            accountToAdd.Add(AccountEntity);
        }

        await _dbContext.Accounts.AddRangeAsync(accountToAdd);
        await _dbContext.SaveChangesAsync();

        foreach (var obj in accounts)
        {
            var subject = "Account Created Successfully!";
            var body = $@"
                <p>Dear {obj.Email},</p>
                <p>Your account has been successfully created on our platform.</p>
                <p><strong>Email:</strong> {obj.Email}</p>
                <p>Best regards,<br>Your Website Team</p>";
            await _emailService.SendEmailAsync(new List<string> { obj.Email }, subject, body);
        }
        return true;
    }

    public async Task<string?> Login(LoginUserAccount obj)
    {
        var user = _dbContext.Accounts.FirstOrDefault(x => x.AccountEmail == obj.Email && x.AccountPassword == obj.Password);
        if (user == null) return null;

        var jwtConfig = _configuration.GetSection("JwtConfig");
        var key = jwtConfig["Key"];
        var expirationMinutes = int.Parse(jwtConfig["TokenValidityMins"]);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.AccountEmail),
            new Claim(ClaimTypes.Role, user.AccountRoleID),
            new Claim("AccountId", user.AccountId.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<bool> DeleteUserById(int id)
    {
        var user = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);
        if (user == null) return false;

        _dbContext.Accounts.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Account>> GetAllAccounts()
    {
        return await _dbContext.Accounts.ToListAsync();
    }

    public async Task<Account?> GetUserByEmail(string email)
    {
        return await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountEmail == email);
    }

    public async Task<bool> ResetPassword(string email, ResetPassword obj)
    {
        var user = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountEmail == email);
        if (user == null) return false;

        if (obj.NewPassword != obj.ConfirmPassword) return false;

        user.AccountPassword = obj.ConfirmPassword;
        await _dbContext.SaveChangesAsync();

        var subject = "Your Password Has Been Successfully Reset";
        var body = $@"
            <p>Dear {email},</p>
            <p>Your password has been successfully reset.</p>
            <p><strong>New password:</strong> {obj.ConfirmPassword}</p>
            <p>Best regards,<br>Your Website Team</p>";
        await _emailService.SendEmailAsync(new List<string> { email }, subject, body);

        return true;
    }

    public async Task<string> FastLogin()
    {
        var anonymousUserId = Guid.NewGuid().ToString();
        var jwtConfig = _configuration.GetSection("JwtConfig");
        var key = jwtConfig["Key"];
        var expirationMinutes = int.Parse(jwtConfig["TokenValidityMins"]);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, anonymousUserId),
            new Claim(ClaimTypes.Role, "Anonymous")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

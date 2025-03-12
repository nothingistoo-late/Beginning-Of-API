using Final4.DTO.Account;
using Final4.IRepository;
using Final4.Model.Entities;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<bool> RegisterUserAccounts(List<RegisterUserAccount> accounts);
    Task<string?> Login(LoginUserAccount obj);
    Task<Account?> GetUserByEmail(string email);
    Task<bool> ResetPassword(string email, ResetPassword obj);
    Task<string> FastLogin();
}

using Final4.Data;
using Final4.DTO.Account;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Final4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        public AccountController(ApplicationDBContext DBContext)
        {
            _dbContext = DBContext;
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
                RoleID = "1"
            };
            _dbContext.Accounts.Add(AccountEntity);
            _dbContext.SaveChanges();
            return Ok(obj);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string email, string password)
        {
            var listUser = _dbContext.Accounts.ToList();
            var checkAccountExits = listUser.FirstOrDefault(x => x.Email == email && x.Password == password);

            if (checkAccountExits != null)
                return Ok();
            else return NotFound();
        }

        [HttpGet]
        [Route("GetAllAccount")]
        public IActionResult GetAccount() 
        {
            //return Ok(_dbContext.Accounts.Select(a=> new {a.Name, a.Email}) .ToList());
            return Ok(_dbContext.Accounts.ToList());
        }
    }
}

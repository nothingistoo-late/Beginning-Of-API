using Final4.Data;
using Final4.DTO.Employees;
using Final4.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        public EmployeesController(ApplicationDBContext DBContext)
        {
            _dbContext = DBContext;
        }
        [HttpGet]
        [Route("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployees()
        {
            return Ok(await _dbContext.Employees.ToListAsync());
        }
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee(AddEmployes obj)
        {
            var EmployeeEntity = new Employee()
            {
                Name = obj.Name,
                Email = obj.Email,
                Phone = obj.Phone,
                Salary = obj.Salary
            };
            _dbContext.Employees.Add(EmployeeEntity);
            await _dbContext.SaveChangesAsync();
            return Ok(EmployeeEntity);
        }

        [HttpGet]
        [Route("SearchEmployeeBy{id:guid}")]
        public async Task<IActionResult> GetEmployeeByID(Guid id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPut]
        [Route("UpdateEmployeeById/{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployee obj)
        {
            var existingEmployee = await _dbContext.Employees.FindAsync(id);
            if (existingEmployee != null)
            {
                // Chỉ cập nhật các trường nếu có giá trị
                if (!string.IsNullOrEmpty(obj.Name))
                    existingEmployee.Name = obj.Name;

                if (!string.IsNullOrEmpty(obj.Email))
                    existingEmployee.Email = obj.Email;

                if (!string.IsNullOrEmpty(obj.Phone))
                    existingEmployee.Phone = obj.Phone;

                if (obj.Salary.HasValue) // Kiểm tra nếu Salary không phải null
                    existingEmployee.Salary = obj.Salary.Value;

                await _dbContext.SaveChangesAsync();
                return Ok(existingEmployee);
            }
            else
            {
                return NotFound("Not Found Employee To Update");
            }
        }

        [HttpDelete]
        [Route("DeleteEmployeeBy{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var Employee = await _dbContext.Employees.FindAsync(id);
            if (Employee == null)
                return NotFound("Not Found Employee To Delete");
            _dbContext.Employees.Remove(Employee);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}

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
        public IActionResult GetAllEmployees()
        {
            return Ok(_dbContext.Employees.ToList());
        }
        [HttpPost]
        [Route("AddEmployee")]
        public IActionResult AddEmployee(AddEmployes obj)
        {
            var EmployeeEntity = new Employee()
            {
                Name = obj.Name,
                Email = obj.Email,
                Phone = obj.Phone,
                Salary = obj.Salary
            };
            _dbContext.Employees.Add(EmployeeEntity);
            _dbContext.SaveChanges();
            return Ok(EmployeeEntity);
        }

        [HttpGet]
        [Route("SearchEmployeeBy{id:guid}")]
        public IActionResult GetEmployeeByID(Guid id)
        {
            var employee = _dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPut]
        [Route("UpdateEmployeeBy{id}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployee obj)
        {
            var existingEmployee = _dbContext.Employees.Find(id);
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

                _dbContext.SaveChanges();
                return Ok(existingEmployee);
            }
            else
            {
                return NotFound("Not Found Employee To Update");
            }
        }

        [HttpDelete]
        [Route("DeleteEmployeeBy{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var Employee = _dbContext.Employees.Find(id);
            if (Employee == null)
                return NotFound("Not Found Employee To Delete");
            _dbContext.Employees.Remove(Employee);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}

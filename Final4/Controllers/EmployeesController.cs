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
            return Ok(_dbContext.Employee.ToList());
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
                Salary = obj.Salary,
            };
            _dbContext.Employee.Add(EmployeeEntity);
            _dbContext.SaveChanges();
            return Ok(EmployeeEntity);
        }

        [HttpGet]
        [Route("SearchEmployeeBy{id:guid}")]
        public IActionResult GetEmployeeByID(Guid id)
        {
            var employee = _dbContext.Employee.Find(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPut]
        [Route("UpdateEmployeeBy{id}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployee obj)
        {
            var UpdateEmployee = _dbContext.Employee.Find(id);
            if (UpdateEmployee != null)
            {
                UpdateEmployee.Name = obj.Name;
                UpdateEmployee.Email = obj.Email;
                UpdateEmployee.Phone = obj.Phone;
                UpdateEmployee.Salary = obj.Salary;
            }
            else
                return NotFound("Not Found Employee To Update");
            _dbContext.SaveChanges();
            return Ok(UpdateEmployee);
        }
        [HttpDelete]
        [Route("DeleteEmployeeBy{id}")]
        public IActionResult DeleteEmployee(Guid id) 
        {
            var Employee = _dbContext.Employee.Find(id);
            if (Employee == null)
                return NotFound("Not Found Employee To Delete");
            _dbContext.Employee.Remove(Employee);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}

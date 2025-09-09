using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentManager.Filters;
using TalentManager.Models;

namespace TalentManager.Controllers
{
    [ApiController]
    [ConcurrencyChecker]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private static readonly Dictionary<int, Employee> _employees = new()
        {
            [123] = new Employee { Id = 123, Name = "Austin T Lee", Department = "IT", Country = "America" },
            [456] = new Employee { Id = 456, Name = "Douglas J Falcon", Department = "Enforcement", Country = "Spain" },
            [789] = new Employee { Id = 789, Name = "Mario M Mario", Department = "Plumbing", Country = "Italy" },
            [001] = new Employee { Id = 001, Name = "Test Delete", Department = "IT", Country = "USA"},
        };


        [HttpGet]
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employees.Values;
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> Get(int id)
        {
            if(_employees.ContainsKey(id))
            {
                return _employees[id];
            }

            return NotFound($"Employee with id {id} not found");
        }

        [HttpPut]
        public IActionResult Put(Employee employee, [FromHeader(Name = "If-Match")] string ifMatch = "")
        {
            if(_employees.ContainsKey(employee.Id))
            {
                _employees[employee.Id] = employee;
                return Ok(employee);
            }
            
            return NotFound($"Employee with id {employee.Id} not found");
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = "Human Resources Manager")]
        public async Task<IActionResult> Delete(
            int id,
            [FromServices] IAuthorizationService authz)
        {
            var user = HttpContext.User;
            //var userDept = user.FindFirst("Department")?.Value;
            //var userCountry = user.FindFirst("Country")?.Value;

            if (!_employees.TryGetValue(id, out var employee))
            {
                return NotFound();
            }

            //if (userDept != employee.Department || userCountry != employee.Country)
            //{
            //    return Forbid();
            //}

            var result = await authz.AuthorizeAsync(User, employee, "CanDeleteEmployee");
            if (!result.Succeeded) return Forbid();

            _employees.Remove(id);
            return Ok($"Employee {id} deleted by {user.Identity?.Name}");

        }
    }
}

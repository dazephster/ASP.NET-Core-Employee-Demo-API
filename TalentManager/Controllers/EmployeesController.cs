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
        [HttpGet]
        public IEnumerable<Employee> GetAllEmployees()
        {
            return new List<Employee>
            {
                new Employee()
                {
                    Id = 123,
                    Name = "Austin T Lee",
                    Department = "IT"
                },
                new Employee()
                {
                    Id = 456,
                    Name = "Douglas J Falcon",
                    Department = "Enforcement"
                },
                new Employee()
                {
                    Id = 789,
                    Name = "Mario M Mario",
                    Department = "Plumbing"
                }
            };
        }

        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return new Employee()
            {
                Id = id,
                Name = "Mario M Mario",
                Department = "Plumbing"
            };
        }

        [HttpPut]
        public IActionResult Put(Employee employee, [FromHeader(Name = "If-Match")] string ifMatch = "")
        {
            return Ok();
        }
    }
}

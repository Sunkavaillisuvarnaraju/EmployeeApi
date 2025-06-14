using Microsoft.AspNetCore.Mvc;
using EmployeeAPI.Data;
using EmployeeAPI.Models;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DbHelper _db;

        public EmployeeController(DbHelper db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_db.GetEmployees());

        [HttpPost]
        public IActionResult Post([FromBody] Employee emp)
        {
            _db.AddEmployee(emp);
            return Ok(new { message = "Employee added" });
        }

        [HttpPut]
        public IActionResult Put([FromBody] Employee emp)
        {
            _db.UpdateEmployee(emp);
            return Ok(new { message = "Employee updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _db.DeleteEmployee(id);
            return Ok(new { message = "Employee deleted" });
        }
    }
}

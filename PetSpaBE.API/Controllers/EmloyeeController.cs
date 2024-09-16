using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ModelViews;
using System;
using System.Threading.Tasks;

namespace PetSpaBE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAll();
            return Ok(employees);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(object id)
        {
            var employee = await _employeeService.GetById(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] EmployeeResponseModel employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _employeeService.Add(employee);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] EmployeeResponseModel employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _employeeService.Update(employee);
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(object id)
        {
            await _employeeService.Delete(id);
            return NoContent();
        }
    }
}

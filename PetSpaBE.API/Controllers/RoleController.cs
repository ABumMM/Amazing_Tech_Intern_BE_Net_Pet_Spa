using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationRole>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationRole>> GetRoleById(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(new { Message = "Role not found" });
            }

            return Ok(role);
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult> AddRole([FromBody] ApplicationRole role)
        {
            if (role == null)
            {
                return BadRequest(new { Message = "Invalid role data" });
            }

            await _roleService.AddAsync(role);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }

        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] ApplicationRole role)
        {
            if (role == null || id != role.Id)
            {
                return BadRequest(new { Message = "Invalid role data or ID mismatch" });
            }

            await _roleService.UpdateAsync(role);
            return NoContent();
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);

            if (role == null)
            {
                return NotFound(new { Message = "Role not found" });
            }

            await _roleService.DeleteAsync(id);
            return NoContent();
        }
    }
}

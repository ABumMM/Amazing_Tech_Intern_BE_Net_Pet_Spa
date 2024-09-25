using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.RoleModelViews;

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

        [HttpGet]
        public async Task<IActionResult> GetAllRoles(int pageNumber = 1, int pageSize = 2)
        {
            var roles = await _roleService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETRoleModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: roles));
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] POSTRoleModelView role)
        {
            await _roleService.Add(role);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add role successful"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid Id)
        {
            await _roleService.Delete(Id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete role successful"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(Guid Id)
        {
            var role = await _roleService.GetById(Id);
            return Ok(new BaseResponseModel<GETRoleModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: role));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole([FromBody] PUTRoleModelView role)
        {
            await _roleService.Update(role);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update role successful"));
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.RoleModelViews;
using PetSpa.ModelViews.UserModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //[Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int pageNumber, int pageSize)
        {
            var users = await _userService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETUserModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: users));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetById(id);

            return Ok(new BaseResponseModel<GETUserModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: user));
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers(int pageNumber, int pageSize)
        {
            var customers = await _userService.GetCustomers(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETUserModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: customers));
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees(int pageNumber, int pageSize)
        {
            var employees = await _userService.GetEmployees(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETUserModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: employees));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] PUTUserModelView user)
        {
            await _userService.Update(user);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update user successful"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            await _userService.Delete(Id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete user successful"));
        }
    }
}

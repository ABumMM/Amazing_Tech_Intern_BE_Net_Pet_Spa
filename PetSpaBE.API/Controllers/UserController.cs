﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Core.Base;
using PetSpa.ModelViews.UserModelViews;

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
        
        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetAllUsers(int pageNumber, int pageSize)
        {
            var users = await _userService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETUserModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: users));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetById(id);

            return Ok(new BaseResponseModel<GETUserModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: user));
        }

        [HttpGet("customers")]
        [Authorize]
        public async Task<IActionResult> GetCustomers(int pageNumber, int pageSize)
        {
            var customers = await _userService.GetCustomers(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETUserModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: customers));
        }

        [HttpGet("employees")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetEmployees(int pageNumber, int pageSize)
        {
            var employees = await _userService.GetEmployees(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETUserModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: employees));
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> UpdateUser([FromBody] PUTUserModelView user)
        {
            await _userService.Update(user);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update user successful"));
        }

        [HttpPut("updatecustomer")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateUserforcustomer([FromBody] PUTuserforcustomer customer)
        {
            await _userService.UpdateForCustomer(customer);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update user successful"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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

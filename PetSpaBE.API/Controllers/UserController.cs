using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ModelViews;
using System.Threading.Tasks;
using System.Linq;
using PetSpa.Core.Base;

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

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(int pageNumber = 1, int pageSize = 10)
        {
            var users = await _userService.GetAll();
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }

            // Thực hiện phân trang
            var paginatedUsers = users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Tạo đối tượng BasePaginatedList để trả về
            var totalUserCount = users.Count;
            var paginatedList = new BasePaginatedList<UserResponseModel>(paginatedUsers, totalUserCount, pageNumber, pageSize);

            return Ok(paginatedList);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            return Ok(user);
        }
    }
}

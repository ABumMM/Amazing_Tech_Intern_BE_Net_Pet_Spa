using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberShipController : ControllerBase
    {
        private readonly IMembershipsService _membershipsService;

        public MemberShipController(IMembershipsService membershipsService)
        {
            _membershipsService = membershipsService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMemberShips(int pageNumber = 1, int pageSize = 2)
        {
            IList<MemberShips> memberShips = await _membershipsService.GetAll();

            int totalMemberShip = memberShips.Count;

            // Thực hiện phân trang
            var paginatedMemberShips = memberShips
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Tạo đối tượng BasePaginatedList để trả về
            var paginatedList = new BasePaginatedList<MemberShips>(paginatedMemberShips, totalMemberShip, pageNumber, pageSize);

            return Ok(paginatedList);
        }
        [HttpPost]
        public async Task<IActionResult> AddMemberShip(MemberShips memberShips)
        {
            await _membershipsService.Add(memberShips);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMemberShip(string id)
        {
            try
            {
                await _membershipsService.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound("MemberShip not found!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberShipById(string id)
        {
            var memberShips = await _membershipsService.GetById(id) ?? null;
            if (memberShips is null)
                return NotFound("MemberShip not found!");
            return Ok(memberShips);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateMemberShip(MemberShips memberShip)
        {
            try
            {
                await _membershipsService.Update(memberShip);
                return Ok();
            }
            catch 
            {
                return NotFound("MemberShip not found!");
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.MemberShipModelView;
using PetSpa.ModelViews.MemberShipModelViews;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.Services.Service;

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
        public async Task<IActionResult> GetAllMemberShips(int pageNumber, int pageSize)
        {
            var memberShips = await _membershipsService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETMemberShipModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: memberShips));
        }
        [HttpPost]
        public async Task<IActionResult> AddMemberShip([FromBody]POSTMemberShipModelView memberShipMV)
        {
            await _membershipsService.Add(memberShipMV);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add membership successful"));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberShip(string id)
        {
            await _membershipsService.Delete(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete membership successful"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberShipById(string id)
        {
            var memberShip = await _membershipsService.GetById(id);
            return Ok(new BaseResponseModel<GETMemberShipModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: memberShip));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMemberShip([FromBody]PUTMemberShipModelView memberShipMV)
        {
            await _membershipsService.Update(memberShipMV);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Update package successful"));
        }
    }
}

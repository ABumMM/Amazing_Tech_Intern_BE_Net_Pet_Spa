using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.MemberShipModelView;
using Swashbuckle.AspNetCore.Annotations;

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

        // GET: api/membership
        [HttpGet]
        [SwaggerOperation(
             Summary = "Authorization: Admin",
            Description = "View All membership"
            )]
        [Authorize(Roles = "Admim")]
        public async Task<IActionResult> GetAllMemberShips(int pageNumber, int pageSize)
        {
            var memberShips = await _membershipsService.GetAll(pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETMemberShipModelView>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: memberShips));
        }

        // GET: api/membership/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(
             Summary = "Authorization: Admin",
            Description = "Get membership by ID"
            )]
        [Authorize(Roles = "Admim")]
        public async Task<IActionResult> GetMemberShipById(string id)
        {
            var memberShip = await _membershipsService.GetById(id);
            return Ok(new BaseResponseModel<GETMemberShipModelView>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: memberShip));
        }


      
    }
}

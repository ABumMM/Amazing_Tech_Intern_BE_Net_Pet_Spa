using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.BookingModelViews;
using PetSpa.ModelViews.RankModelViews;
using PetSpa.Services.Service;

namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private IRankSerivce _rankService;

        public RankController(IRankSerivce rankSerivce) {
            _rankService = rankSerivce;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int PageNumber , int PageSize ) {
            var ranks = await _rankService.GetAll(PageNumber, PageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GetRankViewModel>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: ranks));
        }
        [HttpPost]
        public async Task<IActionResult> Create(PostRankViewModel postRank)
        {
            await _rankService.Add(postRank);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status201Created,
               code: ResponseCodeConstants.SUCCESS,
               data: "Create rank successfull"));
        }
        [HttpPatch]
        public async Task<IActionResult> Upadate(string RankID,UpdateViewModel updateModel)
        {
            await _rankService.Update( RankID,updateModel);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Update rank successfull"));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string RankID) {
            await _rankService.Delete(RankID);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete rank successful"));
        }
    }
}

﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.RankModelViews;
namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private IRankService _rankService;

        public RankController(IRankService rankSerivce) {
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PostRankViewModel postRank)
        {
            await _rankService.Add(postRank);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status201Created,
               code: ResponseCodeConstants.SUCCESS,
               data: "Create rank successfull"));
        }
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upadate(string RankID,UpdateViewModel updateModel)
        {
            await _rankService.Update( RankID,updateModel);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Update rank successfull"));
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string RankID) {
            await _rankService.Delete(RankID);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete rank successful"));
        }
    }
}

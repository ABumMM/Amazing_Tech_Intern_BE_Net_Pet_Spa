using Microsoft.AspNetCore.Mvc;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.ReviewModelViews;
using PetSpa.Services.Service;
namespace PetSpaBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPackages(string packageID,int pageNumber, int pageSize)
        {
            var reviews = await _reviewService.GetAllReviewsInPackage(packageID, pageNumber, pageSize);
            return Ok(new BaseResponseModel<BasePaginatedList<GETReviewModelViews>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: reviews));
        }
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] POSTReviewModelViews reviewMV)
        {
            await _reviewService.Add(reviewMV);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Add review successful"));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteReview(string id)
        {
            await _reviewService.Delete(id);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Delete review successful"));
        }
    }
}

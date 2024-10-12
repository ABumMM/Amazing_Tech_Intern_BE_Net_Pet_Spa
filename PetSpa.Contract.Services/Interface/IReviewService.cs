using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.ReviewModelViews;
namespace PetSpa.Contract.Services.Interface
{
    public interface IReviewService
    {
        Task<BasePaginatedList<GETReviewModelViews>> GetAllReviewsInPackage(string packageID,int pageNumber, int pageSize);
        Task Add(POSTReviewModelViews review);
        Task Delete(string reviewID);
    }
}

using PetSpa.Core.Base;
using PetSpa.ModelViews.RankModelViews;

namespace PetSpa.Contract.Services.Interface
{
    public interface IRankSerivce
    {
        Task<BasePaginatedList<GetRankViewModel>> GetAll(int pageNumber, int pageSize);
        Task Add(PostRankViewModel rank);
        Task Update(string RankID, UpdateViewModel UpdateRank);
        Task Delete(string rankID);
    }
}

using PetSpa.Core.Base;
using PetSpa.ModelViews.RankModelViews;
using PetSpa.ModelViews.ReviewModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

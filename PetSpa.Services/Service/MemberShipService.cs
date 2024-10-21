using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.ModelViews.MemberShipModelView;
using System;
namespace PetSpa.Services.Service
{
    public class MemberShipService:IMembershipsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
        public MemberShipService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor
            ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor= contextAccessor;
            _mapper= mapper;
        }
        public async Task<BasePaginatedList<GETMemberShipModelView>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            IQueryable<MemberShips> memberShips = _unitOfWork.GetRepository<MemberShips>()
                .Entities.Where(i => !i.DeletedTime.HasValue)//Membership chưa bị xóa
                .OrderByDescending(c=>c.CreatedTime).AsQueryable();// Sắp xếp theo thời gian tạo                                                  
                var paginatedMemberShips = await memberShips
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
            return new BasePaginatedList<GETMemberShipModelView>(_mapper.Map<List<GETMemberShipModelView>>(paginatedMemberShips), 
                await memberShips.CountAsync(), pageNumber, pageSize);
        }
        public async Task<GETMemberShipModelView?> GetById(string memberShipID)
        {
            if (string.IsNullOrWhiteSpace(memberShipID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid memberhip ID.");
            }
            var existedMemberShips = await _unitOfWork.GetRepository<MemberShips>().Entities.FirstOrDefaultAsync(p => p.Id == memberShipID) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found membership");
            return _mapper.Map<GETMemberShipModelView>(existedMemberShips);
        }

        public async Task UpdateMemberShip( string OrderID)
        {
            var Order = await _unitOfWork.GetRepository<Orders>().Entities.FirstOrDefaultAsync( h => h.Id == OrderID);
            if (Order != null) {
                
                var newRank = GetNewRank(Order.Total);
                if (!newRank.Equals(""))
                {
                    var MemberShip = _unitOfWork.GetRepository<MemberShips>().Entities.FirstOrDefault(c => c.UserId == Order.CustomerID);
                    if (MemberShip != null)
                    {
                        MemberShip.RankId = newRank;
                        await _unitOfWork.SaveAsync();
                    }
                }
                
            }
            
        }
        public string GetNewRank ( decimal totalPrice)
        {
            var Ranks = _unitOfWork.GetRepository<Rank>().Entities
            .OrderBy(rank => rank.MinPrice)
            .ToList();

            // Lọc các Rank có MinPrice > totalPrice
            string newRank = "";
            for (int i = Ranks.Count - 1; i >= 0; i--) {
                if (totalPrice > Ranks[i].MinPrice)
                { 
                    newRank = Ranks[i].Id;
                    break;
                }
            }
            return newRank;
        }
      
    }
}

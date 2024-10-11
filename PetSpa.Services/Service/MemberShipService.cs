using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.ModelViews.MemberShipModelView;
namespace PetSpa.Services.Service
{
    public class MemberShipService:IMembershipsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
        public MemberShipService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor
            , IMapper mapper)
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
        // Phương thức kiểm tra xem thành viên có đủ điều kiện để nâng hạng không
        public async Task CheckMembershipUpgrade(Guid customerId)
        {
            var membership = await _unitOfWork.GetRepository<MemberShips>().GetByIdAsync(customerId);

            if (membership == null)
            {
                throw new Exception("No active membership found for the customer.");
            }

            if (membership.TotalSpent >= 20000000) // Platinum
            {
                if (membership.Name != "Platinum")
                {
                    membership.Name = "Platinum";
                    membership.DiscountRate = 0.20; // Giảm giá 20% cho hạng Platinum
                }
            }
            else if (membership.TotalSpent >= 10000000) // Gold
            {
                if (membership.Name != "Gold")
                {
                    membership.Name = "Gold";
                    membership.DiscountRate = 0.15; // Giảm giá 15% cho hạng Gold
                }
            }
            else if (membership.TotalSpent >= 5000000) // Silver
            {
                if (membership.Name != "Silver")
                {
                    membership.Name = "Silver";
                    membership.DiscountRate = 0.10; // Giảm giá 10% cho hạng Silver
                }
            }
            else // Standard
            {
                if (membership.Name != "Standard")
                {
                    membership.Name = "Standard";
                    membership.DiscountRate = 0; // Không giảm giá cho hạng Standard
                }
            }

            await _unitOfWork.GetRepository<MemberShips>().UpdateAsync(membership);
        }
    }
}

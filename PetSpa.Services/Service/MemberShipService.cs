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
        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
        public MemberShipService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor= contextAccessor;
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
            //Phân trang và chỉ lấy các bản ghi cần thiết
            var paginatedMemberShips = await memberShips
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(pa => new GETMemberShipModelView
                {
                    Id = pa.Id,
                    Name = pa.Name,
                    DiscountRate = pa.DiscountRate,
                    TotalSpent=pa.TotalSpent,
                    CreatedTime = pa.CreatedTime,
                    CreatedBy = pa.CreatedBy,
                }).ToListAsync();
            return new BasePaginatedList<GETMemberShipModelView>(paginatedMemberShips, await memberShips.CountAsync(), pageNumber, pageSize);
        }
        public async Task<GETMemberShipModelView?> GetById(string memberShipID)
        {
            if (string.IsNullOrWhiteSpace(memberShipID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid memberhip ID.");
            }
            var existedMemberShips = await _unitOfWork.GetRepository<MemberShips>().Entities.FirstOrDefaultAsync(p => p.Id == memberShipID) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found membership");
            return new GETMemberShipModelView
            {
                Id = existedMemberShips.Id,
                Name = existedMemberShips.Name,
                CreatedTime = existedMemberShips.CreatedTime,
                CreatedBy=existedMemberShips.CreatedBy,
            };
        }
        // Phương thức kiểm tra xem thành viên có đủ điều kiện để nâng hạng không
        public async Task<string> CheckMembershipUpgrade(Guid customerId)
        {
            var membership = await _unitOfWork.GetRepository<MemberShips>().GetByIdAsync(customerId);

            if (membership == null)
            {
                throw new Exception("No active membership found for the customer.");
            }

            string currentLevel = membership.Name;

            //PLatinum sử dụng hơn 20000000
            if (membership.TotalSpent >= 20000000 )
            {
                if (currentLevel != "Platinum")
                {
                    membership.Name = "Platinum";
                    await _unitOfWork.SaveAsync();
                    return "Membership upgraded to Platinum!";
                }
            }
            else if (membership.TotalSpent >= 10000000)
            {
                if (currentLevel != "Gold")
                {
                    membership.Name = "Gold";
                    await _unitOfWork.SaveAsync();
                    return "Membership upgraded to Gold!";
                }
            }
            else if (membership.TotalSpent >= 5000000)
            {
                if (currentLevel != "Silver")
                {
                    membership.Name = "Silver";
                    await _unitOfWork.SaveAsync();
                    return "Membership upgraded to Silver!";
                }
            }
            return "No membership upgrade.";
        }
    }
}

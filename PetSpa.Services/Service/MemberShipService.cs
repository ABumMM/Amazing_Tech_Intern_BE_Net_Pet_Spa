using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.MemberShipModelView;
using PetSpa.ModelViews.MemberShipModelViews;
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
        public async Task Add(POSTMemberShipModelView memberShipMV)
        {
            if (memberShipMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Membership cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(memberShipMV.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Membership name is required.");
            }
            if (memberShipMV.Point < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Point must be greater than or equal to 0.");
            }
            MemberShips memberShips = new MemberShips()
            {
                Name = memberShipMV.Name,
                Point = memberShipMV.Point,
                SpecialOffer = memberShipMV.SpecialOffer,
                CreatedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now),
                CreatedBy = currentUserId
            };

            await _unitOfWork.GetRepository<MemberShips>().InsertAsync(memberShips);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string MemberShipId)
        {
            MemberShips? existedMemberShips = await _unitOfWork.GetRepository<MemberShips>().GetByIdAsync(MemberShipId);
            if (existedMemberShips == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
            existedMemberShips.DeletedTime = DateTime.Now;
            existedMemberShips.DeletedBy = "ehehehheh";
            await _unitOfWork.GetRepository<MemberShips>().UpdateAsync(existedMemberShips);
            await _unitOfWork.SaveAsync();
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
                    Point = pa.Point,
                    SpecialOffer = pa.SpecialOffer,
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
                Point = existedMemberShips.Point,
                SpecialOffer = existedMemberShips.SpecialOffer,
                CreatedTime = existedMemberShips.CreatedTime
            };
        }
        public Task<List<GETMemberShipModelView?>> GetMemberShips(string packageID, DateTime? DateStart, DateTime? EndStart, string? Name)
        {
            throw new NotImplementedException();
        }
        public async Task Update(PUTMemberShipModelView memberShipMV)
        {
            // Kiểm tra nếu packageMV null
            if (memberShipMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "MemberShip cannot be null.");
            }
            // Kiểm tra nếu Id bị thiếu hoặc không hợp lệ
            if (memberShipMV.Id == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "MemberShip id is required.");
            }
            MemberShips? existedMemberShip = await _unitOfWork.GetRepository<MemberShips>().GetByIdAsync(memberShipMV.Id);
            if (existedMemberShip == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found MemberShip");
            }
            if (memberShipMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "MemberShip cannot null.");
            }
            if (memberShipMV.Name == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "MemberShip name is required.");
            }
            if (memberShipMV.Point < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Point must be greater than or equal to 0.");
            }
            existedMemberShip.Name = memberShipMV.Name;
            existedMemberShip.Point = memberShipMV.Point;
            existedMemberShip.SpecialOffer = memberShipMV.SpecialOffer;
            existedMemberShip.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<MemberShips>().UpdateAsync(existedMemberShip);
            await _unitOfWork.SaveAsync();
        }
    }
}

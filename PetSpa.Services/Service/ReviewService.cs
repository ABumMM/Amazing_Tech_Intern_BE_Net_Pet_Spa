using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.ReviewModelViews;
namespace PetSpa.Services.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);

        public ReviewService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }
        public async Task<BasePaginatedList<GETReviewModelViews>> GetAllReviewsInPackage(string packageID, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(packageID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "PackageID cannot null or whitespace");
            }
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            IQueryable<Reviews> reviews = _unitOfWork.GetRepository<Reviews>()
                   .Entities.Where(i => !i.DeletedTime.HasValue && i.PackageID == packageID) 
                   .OrderByDescending(c => c.CreatedTime) // Sắp xếp theo thời gian tạo
                   .AsQueryable();
            //Phân trang và chỉ lấy các bản ghi cần thiết
            var paginatedReviews = await reviews
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
            return new BasePaginatedList<GETReviewModelViews>(_mapper.Map<List<GETReviewModelViews>>(paginatedReviews),
                await reviews.CountAsync(), pageNumber, pageSize);
        }
        public async Task Add(POSTReviewModelViews reviewMV)
        {
            //Nếu đi theo đặt trực tiếp không qua booking:Order thiếu thuộc tính CustomerID
            var completedOrderDetail = await _unitOfWork.GetRepository<OrdersDetails>().Entities
                 .Include(od => od.Order) // Bao gồm thông tin đơn hàng
                 .Include(od => od.Package) // Nếu cần thông tin về gói dịch vụ
                 .Where(od => od.PackageID == reviewMV.PackageID
                              && od.Order != null // Đảm bảo Orders không phải là null
                              && od.Order.CustomerID ==Guid.Parse(currentUserId)
                              && od.Order.IsPaid) // Kiểm tra trạng thái đơn hàng nếu cần
                 .FirstOrDefaultAsync();
            if (completedOrderDetail == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, 
                    "You cannot review this package as it has not been completed in any of your orders");
            }
            // Thêm đánh giá mới vào hệ thống
            var review = new Reviews
            {
                Description=reviewMV.Description,
                PackageID=reviewMV.PackageID,
                CreatedBy=currentUserId,
                CreatedTime= TimeHelper.ConvertToUtcPlus7(DateTime.Now),
            };

            await _unitOfWork.GetRepository<Reviews>().InsertAsync(review);
            await _unitOfWork.SaveAsync();
        }
        public async Task Delete(string reviewID)
        {
            if (string.IsNullOrWhiteSpace(reviewID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "ReviewID cannot null or whitespace"); throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "PackageID cannot null or whitespace");
            }
            Reviews? existedReview = await _unitOfWork.GetRepository<Reviews>().GetByIdAsync(reviewID);
            if (existedReview == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Review");
            }

            existedReview.DeletedTime = TimeHelper.ConvertToUtcPlus7(DateTime.Now);
            existedReview.DeletedBy = currentUserId;
            await _unitOfWork.GetRepository<Reviews>().UpdateAsync(existedReview);
            await _unitOfWork.SaveAsync();
        }

      
    }
}

using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.ModelViews;

namespace PetSpa.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<UserResponseModel>> GetAll()
        {
            // Lấy tất cả người dùng từ repository
            var users = await _unitOfWork.GetRepository<ApplicationUser>().GetAllAsync();

            // Chuyển đổi từ ApplicationUser sang UserResponseModel
            var userResponseModels = users.Select(user => new UserResponseModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                FullName = user.UserInfo?.FullName,
                BankAccount = user.UserInfo?.BankAccount,
                BankAccountName = user.UserInfo?.BankAccountName,
                Bank = user.UserInfo?.Bank,
                CreatedBy = user.CreatedBy,
                LastUpdatedBy = user.LastUpdatedBy,
                DeletedBy = user.DeletedBy,
                CreatedTime = user.CreatedTime,
                LastUpdatedTime = user.LastUpdatedTime,
                DeletedTime = user.DeletedTime
            }).ToList();

            return userResponseModels;
        }

        public async Task<UserResponseModel?> GetById(object id)
        {
            // Lấy người dùng theo ID từ repository
            var user = await _unitOfWork.GetRepository<ApplicationUser>().GetByIdAsync(id);

            // Chuyển đổi từ ApplicationUser sang UserResponseModel nếu người dùng tồn tại
            if (user == null)
                return null;

            return new UserResponseModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                FullName = user.UserInfo?.FullName,
                BankAccount = user.UserInfo?.BankAccount,
                BankAccountName = user.UserInfo?.BankAccountName,
                Bank = user.UserInfo?.Bank,
                CreatedBy = user.CreatedBy,
                LastUpdatedBy = user.LastUpdatedBy,
                DeletedBy = user.DeletedBy,
                CreatedTime = user.CreatedTime,
                LastUpdatedTime = user.LastUpdatedTime,
                DeletedTime = user.DeletedTime
            };
        }
    }
}

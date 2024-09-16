/*using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.ModelViews.UserResponseModel;
using System;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class AuthService : IAuthService
    {
        // Giả sử có một _unitOfWork để làm việc với cơ sở dữ liệu
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(UserResponseModel user)
        {
            // Chuyển đổi từ UserResponseModel sang ApplicationUser
            var applicationUser = new ApplicationUser
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                // Các thuộc tính khác cần được ánh xạ tương tự
                CreatedBy = user.CreatedBy,
                LastUpdatedBy = user.LastUpdatedBy,
                DeletedBy = user.DeletedBy,
                CreatedTime = user.CreatedTime,
                LastUpdatedTime = user.LastUpdatedTime,
                DeletedTime = user.DeletedTime
            };

            // Thêm vào cơ sở dữ liệu
            user.Id = Guid.NewGuid().ToString("N");
            IGenericRepository<Packages> genericRepository = _unitOfWork.GetRepository<Packages>();
            await genericRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task Update(UserResponseModel user)
        {
            // Chuyển đổi từ UserResponseModel sang ApplicationUser
            var applicationUser = new ApplicationUser
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                // Các thuộc tính khác cần được ánh xạ tương tự
                CreatedBy = user.CreatedBy,
                LastUpdatedBy = user.LastUpdatedBy,
                DeletedBy = user.DeletedBy,
                CreatedTime = user.CreatedTime,
                LastUpdatedTime = user.LastUpdatedTime,
                DeletedTime = user.DeletedTime
            };

            // Cập nhật thông tin người dùng trong cơ sở dữ liệu
            var userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            userRepository.Update(applicationUser);
            await _unitOfWork.CommitAsync();
        }

        public async Task Delete(object id)
        {
            // Xóa người dùng theo ID
            var userId = Guid.Parse(id.ToString());
            var userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            var user = await userRepository.GetByIdAsync(userId);

            if (user != null)
            {
                userRepository.Delete(user);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
*/
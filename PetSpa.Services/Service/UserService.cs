using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.UserModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);
            return Guid.Parse(userIdString);
        }

        private string GetCurrentUserRole()
        {
            return Authentication.GetUserRoleFromHttpContext(_httpContextAccessor.HttpContext);
        }

        public async Task<GETUserModelView> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id không hợp lệ.", nameof(id));
            }

            var userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            var user = await userRepository.Entities
                .Include(u => u.UserInfo)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException("Người dùng không tìm thấy.");
            }

            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;

            return new GETUserModelView
            {
                Id = user.Id,
                Email = user.Email,
                RoleName = roleName,
                UserInfo = user.UserInfo ?? new UserInfo(),
                CreatedBy = user.CreatedBy,
                CreatedTime = user.CreatedTime,
            };
        }

        public async Task<BasePaginatedList<GETUserModelView>> GetAll(int pageNumber = 1, int pageSize = 3)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException("Số trang phải lớn hơn 0.", nameof(pageNumber));
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("Kích thước trang phải lớn hơn 0.", nameof(pageSize));
            }

            var userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            var users = await userRepository.Entities
                .Include(u => u.UserInfo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int totalUsers = await userRepository.Entities.CountAsync();

            var userModelViews = users.Select(async user =>
            {
                var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;

                return new GETUserModelView
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleName = roleName,
                    UserInfo = user.UserInfo ?? new UserInfo(),
                    CreatedBy = user.CreatedBy,
                    CreatedTime = user.CreatedTime,
                };
            });

            return new BasePaginatedList<GETUserModelView>(await Task.WhenAll(userModelViews), totalUsers, pageNumber, pageSize);
        }

        public async Task<PUTUserModelView> Update(PUTUserModelView user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Thông tin người dùng không được để trống.");
            }
            if (user.Id == Guid.Empty)
            {
                throw new ArgumentException("Id không hợp lệ.", nameof(user.Id));
            }

            var userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            var existingUser = await userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("Người dùng không tìm thấy.");
            }

            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole != "Admin" && currentUserId != user.Id)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền chỉnh sửa thông tin người dùng này.");
            }

            
            existingUser.Email = user.Email;
            existingUser.UserInfo = user.UserInfo; 
            existingUser.LastUpdatedBy = user.LastUpdatedBy;
            existingUser.LastUpdatedTime = user.LastUpdatedTime;

            await userRepository.UpdateAsync(existingUser);
            await _unitOfWork.SaveAsync();

            return user;
        }

        public async Task Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id không hợp lệ.", nameof(id));
            }

            var userRepository = _unitOfWork.GetRepository<ApplicationUser>();
            var existedUser = await userRepository.GetByIdAsync(id);
            if (existedUser == null)
            {
                throw new KeyNotFoundException("Người dùng không tìm thấy.");
            }

            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa người dùng này.");
            }

            existedUser.DeletedTime = DateTime.UtcNow;
            await userRepository.UpdateAsync(existedUser);
            await _unitOfWork.SaveAsync();
        }
    }
}

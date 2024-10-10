using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.ModelViews.UserModelViews;

namespace PetSpa.Services.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);

            if (string.IsNullOrWhiteSpace(userIdString))
            {
                throw new UnauthorizedAccessException("User ID is not available in the current context.");
            }

            return Guid.Parse(userIdString);
        }

        private string GetCurrentUserRole()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new UnauthorizedAccessException("HttpContext is not available.");
            }

            return Authentication.GetUserRoleFromHttpContext(httpContext);
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
                .FirstOrDefaultAsync(u => u.Id == id && u.DeletedTime == null);

            if (user == null)
            {
                throw new KeyNotFoundException("Người dùng không tìm thấy.");
            }

            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;

            var userModelView = _mapper.Map<GETUserModelView>(user);
            userModelView.RoleName = roleName;

            return userModelView;
        }

        public async Task<BasePaginatedList<GETUserModelView>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must be greater than 0");
            }

            var usersQuery = _unitOfWork.GetRepository<ApplicationUser>()
                .Entities
                .Include(u => u.UserInfo) 
                .Where(i => !i.DeletedTime.HasValue)
                .OrderByDescending(c => c.CreatedTime);

            var totalUsers = await usersQuery.CountAsync();

            var paginatedUsers = await usersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userModelViews = paginatedUsers.Select(async user =>
            {
                var userModelView = _mapper.Map<GETUserModelView>(user);
                userModelView.RoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;

                userModelView.FullName = user.UserInfo?.FullName ?? string.Empty; 

                return userModelView;
            });

            var userModelViewsList = await Task.WhenAll(userModelViews);

            return new BasePaginatedList<GETUserModelView>(userModelViewsList.ToList(), totalUsers, pageNumber, pageSize);
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
            if (existingUser == null || existingUser.DeletedTime != null)
            {
                throw new KeyNotFoundException("Người dùng không tìm thấy.");
            }

            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole != "Admin" && currentUserId != user.Id)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền chỉnh sửa thông tin người dùng này.");
            }

            _mapper.Map(user, existingUser);

            existingUser.LastUpdatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            existingUser.LastUpdatedTime = DateTime.UtcNow;

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
            if (existedUser == null || existedUser.DeletedTime != null)
            {
                throw new KeyNotFoundException("Người dùng không tìm thấy.");
            }

            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa người dùng này.");
            }

            // xóa mềm chỉ cập nhật thời gian xóa và trạng thái người dùng nhớ nha ae :))
            existedUser.DeletedTime = DateTime.UtcNow;
            // này lấy role name trong http ném vào mấy cái trên cũng vậy
            existedUser.DeletedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            await userRepository.UpdateAsync(existedUser);
            await _unitOfWork.SaveAsync();
        }
    }
}

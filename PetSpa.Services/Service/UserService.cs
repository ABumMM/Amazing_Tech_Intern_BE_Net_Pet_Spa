using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.RoleModelViews;
using PetSpa.ModelViews.UserModelViews;

namespace PetSpa.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BasePaginatedList<GETUserModelView>> GetAll(int pageNumber = 1, int pageSize = 3)
        {
            var users = await _unitOfWork.GetRepository<ApplicationUser>().GetAllAsync();

            var userViewModels = users.Select(us => new GETUserModelView
            {
                Id = us.Id,
                Email = us.Email,
                FullName = us.UserInfo.FullName,
                Salary = us.UserInfo.Salary,
                BankAccount = us.UserInfo.BankAccount,
                BankAccountName = us.UserInfo.BankAccountName,
                Bank = us.UserInfo.Bank,
                CreatedBy = us.CreatedBy,
                CreatedTime = us.CreatedTime,

            }).ToList();
            //Count Package
            int totalUser = users.Count;

            var paginatedUsers = userViewModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETUserModelView>(paginatedUsers, totalUser, pageNumber, pageSize);
        }

        public async Task<GETUserModelView?> GetById(Guid userID)
        {
            if (userID == Guid.Empty)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid user ID.");
            }

            var existedUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities.FirstOrDefaultAsync(u => u.Id == userID);
            if (existedUser == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found User");
            }

            return new GETUserModelView
            {
                Id = existedUser.Id,
                Email = existedUser.Email,
                FullName = existedUser.UserInfo.FullName,
                Salary = existedUser.UserInfo.Salary,
                BankAccount = existedUser.UserInfo.BankAccount,
                BankAccountName = existedUser.UserInfo.BankAccountName,
                Bank = existedUser.UserInfo.Bank,
                CreatedBy = existedUser.CreatedBy,
                CreatedTime = existedUser.CreatedTime,
            };
        }

        public async Task Update(PUTUserModelView user)
        {
            ApplicationUser? existedUser = await _unitOfWork.GetRepository<ApplicationUser>().GetByIdAsync(user.Id);
            if (existedUser == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found User");
            }
            if (user == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "User cannot null.");
            }
            if (user.FullName == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "User fullname is required.");
            }
            existedUser.Email = user.Email;
            existedUser.UserInfo.FullName = user.FullName;
            existedUser.PhoneNumber = user.PhoneNumber;
            existedUser.UserInfo.DayofBirth = user.DayofBirth;
            existedUser.UserInfo.Address = user.Address;
            existedUser.UserInfo.Salary = user.Salary;
            existedUser.UserInfo.BankAccount = user.BankAccount;
            existedUser.UserInfo.BankAccountName = user.BankAccountName;
            existedUser.UserInfo.Bank = user.Bank;
            existedUser.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<ApplicationUser>().UpdateAsync(existedUser);
            await _unitOfWork.SaveAsync();
        }
        public async Task Delete(Guid Id)
        {
            ApplicationUser? existedUser = await _unitOfWork.GetRepository<ApplicationUser>().GetByIdAsync(Id);
            if (existedUser == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found User");
            }
            existedUser.DeletedTime = DateTime.Now;
            //existedUser.DeletedBy = ehehehheh;    //sửa này sau khi thêm auth
            await _unitOfWork.GetRepository<ApplicationUser>().DeleteAsync(Id);
            await _unitOfWork.SaveAsync();
        }

    }
}


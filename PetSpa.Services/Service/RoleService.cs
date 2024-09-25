using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.RoleModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{

    public class RoleService : IRoleService
    {
        /*private string currentUserId => Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);*/
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(POSTRoleModelView Role)
        {
            if (Role == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Role cannot null.");
            }
            if (Role.Name == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Role name is required.");
            }

            ApplicationRole roles = new ApplicationRole()
            {
                Name = Role.Name,
                CreatedBy = Role.CreatedBy, 
                CreatedTime = DateTime.Now,
            };
            await _unitOfWork.GetRepository<ApplicationRole>().InsertAsync(roles);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(Guid Id)
        {
            ApplicationRole? existedRole = await _unitOfWork.GetRepository<ApplicationRole>().GetByIdAsync(Id);
            if (existedRole == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Role");
            }
            existedRole.DeletedTime = DateTime.Now;
            //existedRole.DeletedBy = ehehehheh;    //sửa này sau khi thêm auth
            await _unitOfWork.GetRepository<ApplicationRole>().DeleteAsync(Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETRoleModelView>> GetAll(int pageNumber = 1, int pageSize = 3)
        {
            var roles = await _unitOfWork.GetRepository<ApplicationRole>().GetAllAsync();

            var roleModelViews = roles.Select(r => new GETRoleModelView
            {
                Id = r.Id,
                Name = r.Name,
                CreatedTime = r.CreatedTime,

            }).ToList();
            //Count Package
            int totalRole = roles.Count;

            var paginatedRoles = roleModelViews
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETRoleModelView>(paginatedRoles, totalRole, pageNumber, pageSize);
        }

        public async Task<GETRoleModelView?> GetById(Guid Id)
        {
            var existedRole = await _unitOfWork.GetRepository<ApplicationRole>()
                .Entities.FirstOrDefaultAsync(r => r.Id == Id);
            if (existedRole == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Role");
            }
            return new GETRoleModelView
            {
                Id = existedRole.Id,
                Name = existedRole.Name,

                //THiếu user => chưa làm createby,deleteby,updateby
                //CreatedBy=pa.CreatedBy,
                //LastUpdatedBy=pa.LastUpdatedBy,
                //DeletedBy=pa.DeletedBy,

            };

        }

        public async Task Update(PUTRoleModelView role)
        {
            ApplicationRole? existedRole = await _unitOfWork.GetRepository<ApplicationRole>().GetByIdAsync(role.Id);
            if (existedRole == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found role");
            }
            if (role == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Role cannot null.");
            }
            if (role.Name == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Role name is required.");
            }
            existedRole.Name = role.Name;
            existedRole.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<ApplicationRole>().UpdateAsync(existedRole);
            await _unitOfWork.SaveAsync();
        }   

    }
}

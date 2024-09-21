using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.MemberShipModelView;
using PetSpa.ModelViews.MemberShipModelViews;
using PetSpa.ModelViews.ModelViews;
using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class MemberShipService:IMembershipsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberShipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                CreatedTime = DateTime.Now,
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
            //existedPackage.DeletedBy = ehehehheh;
            await _unitOfWork.GetRepository<MemberShips>().DeleteAsync(MemberShipId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETMemberShipModelView>> GetAll(int pageNumber = 1, int pageSize = 3)
        {
            var memberShips = await _unitOfWork.GetRepository<MemberShips>().GetAllAsync();

            var memberShipViewModels = memberShips.Select(pa => new GETMemberShipModelView
            {
                Id = pa.Id,
                Name = pa.Name,
                Point=pa.Point,
                SpecialOffer=pa.SpecialOffer,
                CreatedTime = pa.CreatedTime,
                //THiếu user => chưa làm createby,deleteby,updateby
                //CreatedBy=pa.CreatedBy,
                //LastUpdatedBy=pa.LastUpdatedBy,
                //DeletedBy=pa.DeletedBy,
            }).ToList();
            //Count Package
            int totalPackage = memberShips.Count;

            var paginatedMemberShips = memberShipViewModels
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETMemberShipModelView>(paginatedMemberShips, totalPackage, pageNumber, pageSize);
        }

        public async Task<GETMemberShipModelView?> GetById(string memberShipID)
        {
            if (string.IsNullOrWhiteSpace(memberShipID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid package ID.");
            }
            var existedMemberShips = await _unitOfWork.GetRepository<MemberShips>().Entities.FirstOrDefaultAsync(p => p.Id == memberShipID);
            if (existedMemberShips == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Package");
            }
            return new GETMemberShipModelView
            {
                Id = existedMemberShips.Id,
                Name = existedMemberShips.Name,
                Point = existedMemberShips.Point,
                SpecialOffer = existedMemberShips.SpecialOffer,
                CreatedTime = existedMemberShips.CreatedTime,
                //THiếu user => chưa làm createby,deleteby,updateby
                //CreatedBy=pa.CreatedBy,
                //LastUpdatedBy=pa.LastUpdatedBy,
                //DeletedBy=pa.DeletedBy,
               
            };

        }

        public Task<List<GETMemberShipModelView?>> GetMemberShips(string packageID, DateTime? DateStart, DateTime? EndStart, string? Name)
        {
            throw new NotImplementedException();
        }

        public Task Update(Packages package)
        {
            throw new NotImplementedException();
        }
    }
}

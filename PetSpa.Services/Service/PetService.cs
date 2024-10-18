using PetSpa.Contract.Repositories.Entity;
using Microsoft.AspNetCore.Http;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PetsModelViews;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Services.Interface;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PetSpa.Services.Mapper;
using PetSpa.Repositories.UOW;
using Microsoft.AspNetCore.Identity;
using PetSpa.ModelViews.UserModelViews;
using PetSpa.ModelViews.PackageModelViews;

namespace PetSpa.Services.Service
{
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public PetService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Add(POSTPetsModelView petMV)
        {
            if (petMV.Name == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pet Name cannot be null.");
            }
            if (petMV.UserId == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "UserId cannot be null.");
            }

            await _unitOfWork.GetRepository<Pets>().InsertAsync(_mapper.Map<Pets>(petMV));
            await _unitOfWork.SaveAsync();


        }
    
        public async Task<PUTPetsModelView> Update(PUTPetsModelView petMV)
        {
            if (string.IsNullOrWhiteSpace(petMV.Id))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "petID cannot be null or whitespace");
            }

            Pets? existedPets = await _unitOfWork.GetRepository<Pets>().GetByIdAsync(petMV.Id);
            if (existedPets == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Pet not found.");
            }

            if (petMV.Price < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Price must be greater than or equal to 0.");
            }
            _mapper.Map(petMV, existedPets);
            await _unitOfWork.GetRepository<Pets>().UpdateAsync(existedPets);
            await _unitOfWork.SaveAsync();
            return petMV;  
        }


        public async Task Delete(string id)
        {
            if (id == string.Empty)
            {
                throw new ArgumentException("Id không hợp lệ.", nameof(id));
            }

            var petRepository = _unitOfWork.GetRepository<Pets>();
            var existedPet = await petRepository.GetByIdAsync(id);
            if (existedPet == null || existedPet.DeletedTime != null)
            {
                throw new KeyNotFoundException("Thú cưng không tìm thấy.");
            }


            // Xóa mềm: cập nhật thời gian xóa và người xóa
            existedPet.DeletedTime = DateTime.UtcNow;
            existedPet.DeletedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            await petRepository.UpdateAsync(existedPet);
            await _unitOfWork.SaveAsync();
        }

        public async Task<GETPetsModelView> GetById(string petsID)
        {
            if (string.IsNullOrWhiteSpace(petsID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid Pets ID.");
            }
            var existedPets = await _unitOfWork.GetRepository<Pets>()
                      .Entities
                      .FirstOrDefaultAsync(p => p.Id == petsID && !p.DeletedTime.HasValue);
            if (existedPets == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Pet");
            }
            return _mapper.Map<GETPetsModelView>(existedPets);
        }


        public async Task<BasePaginatedList<GETPetsModelView>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            IQueryable<Pets> pets = _unitOfWork.GetRepository<Pets>()
               .Entities.Where(i => !i.DeletedTime.HasValue)//Membership chưa bị xóa
               .OrderByDescending(c => c.CreatedTime).AsQueryable();// Sắp xếp theo thời gian tạo
            //Phân trang và chỉ lấy các bản ghi cần thiết
            var paginatedPets = await pets
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();
            return new BasePaginatedList<GETPetsModelView>(_mapper.Map<List<GETPetsModelView>>(paginatedPets),
                await pets.CountAsync(), pageNumber, pageSize);
        }
    }
}
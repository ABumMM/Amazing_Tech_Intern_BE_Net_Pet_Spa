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
        public async Task<PUTPetsModelView> Update(PUTPetsModelView pets)
        {
            if (pets.Id == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid pet data.");
            }

            var petRepository = _unitOfWork.GetRepository<Pets>();
            var existingPet = await petRepository.GetByIdAsync(pets.Id);

            if (existingPet == null || existingPet.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Pet not found.");
            }

            _mapper.Map(pets, existingPet);
            existingPet.LastUpdatedTime = DateTime.UtcNow;

            await petRepository.UpdateAsync(existingPet);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PUTPetsModelView>(existingPet);
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


        public async Task<GETPetsModelView> GetById(string id)
        {
            if (id == string.Empty)
            {
                throw new ArgumentException("Id không hợp lệ.", nameof(id));
            }

            var petsRepository = _unitOfWork.GetRepository<Pets>();
            var pets = await petsRepository.Entities
                .FirstOrDefaultAsync(u => u.Id == id && u.DeletedTime == null);

            if (pets == null)
            {
                throw new KeyNotFoundException("Thú cưng không tìm thấy.");
            }
            return _mapper.Map<GETPetsModelView>(pets);
        }
        public async Task<BasePaginatedList<GETPetsModelView>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must be greater than 0");
            }

            var petsQuery = _unitOfWork.GetRepository<Pets>()
                .Entities
                .Where(i => !i.DeletedTime.HasValue)
                .OrderByDescending(c => c.CreatedTime);

            var totalPets = await petsQuery.CountAsync();

            var paginatedPets = await petsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Chuyển đổi từ Pets sang GETPetsModelView bằng AutoMapper
            var petsModelViewsList = _mapper.Map<List<GETPetsModelView>>(paginatedPets);

            return new BasePaginatedList<GETPetsModelView>(petsModelViewsList, totalPets, pageNumber, pageSize);
        }


    }
}
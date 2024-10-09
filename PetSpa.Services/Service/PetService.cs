using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.Core.Infrastructure;
using PetSpa.ModelViews.PetsModelViews;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PetSpa.Services.Service
{
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PetService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
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

        public async Task<GETPetsModelView> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id không hợp lệ.", nameof(id));
            }

            var petRepository = _unitOfWork.GetRepository<Pets>();
            var pet = await petRepository.Entities
                .FirstOrDefaultAsync(p => p.Id == id && p.DeletedTime == null);

            if (pet == null)
            {
                throw new KeyNotFoundException("Thú cưng không tìm thấy.");
            }

            var petModelView = _mapper.Map<GETPetsModelView>(pet);

            return petModelView;
        }

        public async Task<BasePaginatedList<GETPetsModelView>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("PageNumber và PageSize phải lớn hơn 0.");
            }

            var petsQuery = _unitOfWork.GetRepository<Pets>()
                .Entities
                .Where(p => p.DeletedTime == null)
                .OrderByDescending(p => p.CreatedTime);

            var totalPets = await petsQuery.CountAsync();

            var paginatedPets = await petsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var petModelViews = paginatedPets.Select(pet =>
            {
                return _mapper.Map<GETPetsModelView>(pet);
            }).ToList();

            return new BasePaginatedList<GETPetsModelView>(petModelViews, totalPets, pageNumber, pageSize);
        }

        public async Task<POSTPetsModelView> Add(POSTPetsModelView petMV)
        {
            if (petMV == null)
            {
                throw new ArgumentNullException(nameof(petMV), "Thông tin thú cưng không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(petMV.Name))
            {
                throw new ArgumentException("Tên thú cưng là bắt buộc.", nameof(petMV.Name));
            }

            var pet = _mapper.Map<Pets>(petMV);
            pet.CreatedTime = DateTime.UtcNow;

            await _unitOfWork.GetRepository<Pets>().InsertAsync(pet);
            await _unitOfWork.SaveAsync();

            return petMV;
        }

        public async Task<PUTPetsModelView> Update(PUTPetsModelView petMV)
        {
            if (petMV == null)
            {
                throw new ArgumentNullException(nameof(petMV), "Thông tin thú cưng không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(petMV.Id))
            {
                throw new ArgumentException("Id không hợp lệ.", nameof(petMV.Id));
            }

            var petRepository = _unitOfWork.GetRepository<Pets>();
            var existingPet = await petRepository.GetByIdAsync(petMV.Id);
            if (existingPet == null || existingPet.DeletedTime != null)
            {
                throw new KeyNotFoundException("Thú cưng không tìm thấy.");
            }

            _mapper.Map(petMV, existingPet);
            existingPet.LastUpdatedTime = DateTime.UtcNow;

            await petRepository.UpdateAsync(existingPet);
            await _unitOfWork.SaveAsync();

            return petMV;
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id không hợp lệ.", nameof(id));
            }

            var petRepository = _unitOfWork.GetRepository<Pets>();
            var existedPet = await petRepository.GetByIdAsync(id);
            if (existedPet == null || existedPet.DeletedTime != null)
            {
                throw new KeyNotFoundException("Thú cưng không tìm thấy.");
            }

            existedPet.DeletedTime = DateTime.UtcNow;

            await petRepository.UpdateAsync(existedPet);
            await _unitOfWork.SaveAsync();
        }

        Task IPetService.Add(POSTPetsModelView petMV)
        {
            throw new NotImplementedException();
        }

        Task IPetService.Update(PUTPetsModelView petMV)
        {
            throw new NotImplementedException();
        }
    }
}

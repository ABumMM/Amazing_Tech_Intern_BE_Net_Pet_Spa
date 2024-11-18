using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Base;
using PetSpa.ModelViews.RankModelViews;

namespace PetSpa.Services.Service
{
    public class RankService : IRankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RankService( IUnitOfWork unitOfWork , IMapper mapper ) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(PostRankViewModel rank)
        {
            if (string.IsNullOrEmpty(rank.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Rank name cannot be null or empty.");
            }
            if (rank.MinPrice < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Min price cannot be less than 0.");
            }
            if (rank.DiscountPercent < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Discount percent cannot be null or empty.");
            }
            // Kiểm tra trùng tên
            var existingService = await _unitOfWork.GetRepository<Rank>().Entities.FirstOrDefaultAsync(s => s.Name.ToLower() == rank.Name.ToLower());


            if (existingService != null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Service name already exists.");
            }
            // Kiểm tra trùng minprice
            var existingPoint = await _unitOfWork.GetRepository<Rank>().Entities.FirstOrDefaultAsync(s => s.MinPrice == rank.MinPrice);

            if (existingPoint != null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Service min points already exists");
            }
            var newRank = _mapper.Map<Rank>(rank);
            await _unitOfWork.GetRepository<Rank>().InsertAsync(newRank);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(string rankID)
        {
            if (string.IsNullOrWhiteSpace(rankID))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Invalid service ID.");
            }
            IGenericRepository<Rank> genericRepository = _unitOfWork.GetRepository<Rank>();
            Rank? rank = genericRepository.GetById(rankID);
            if (rank == null)
            {
                throw new ErrorException(statusCode: StatusCodes.Status404NotFound, errorCode: ErrorCode.NotFound, "Not found Service with id =" + rankID);
            }
            // service.DeletedBy = currentUserId;
            rank.DeletedTime = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GetRankViewModel>> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pagenumber and pagesize must greater than 0");
            }
            // Retrieve the repository for ServicesEntity
            var genericRepository = _unitOfWork.GetRepository<Rank>();

            // Create a query for the services that are not marked as deleted
            IQueryable<Rank> rankQuery = genericRepository.Entities
                .Where(s => s.DeletedTime.HasValue == false); // Filter out soft-deleted records

            // Use the pagination method to get the paginated list of services
            BasePaginatedList<Rank> paginatedServices = await genericRepository.GetPagging(rankQuery, pageNumber, pageSize);

            // Map the ServicesEntity to ServiceResponseModel
            var rankResponseModels = paginatedServices.Items.Select(s => _mapper.Map<GetRankViewModel>(s)).ToList();

            // Return the paginated list of service response models
            //return new BasePaginatedList<ServiceResposeModel>(serviceResponseModels, paginatedServices.TotalItems, pageNumber, pageSize);
            return new BasePaginatedList<GetRankViewModel>(rankResponseModels, paginatedServices.TotalItems, pageNumber,pageSize);
        }

        public async Task Update(string RankID, UpdateViewModel UpdateRank)
        {
            if (string.IsNullOrEmpty(UpdateRank.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Rank name cannot be null or empty.");
            }
            if (UpdateRank.MinPrice < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Min price cannot be less than 0.");
            }
            if (UpdateRank.DiscountPercent < 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Discount percent cannot be null or empty.");
            }
            IGenericRepository<Rank> genericRepository = _unitOfWork.GetRepository<Rank>();

            Rank? rank = await _unitOfWork.GetRepository<Rank>().Entities.FirstOrDefaultAsync(s => s.DeletedTime.HasValue == false && s.Id == RankID);
            if (rank == null)
            {
                throw new ErrorException(statusCode: StatusCodes.Status404NotFound, errorCode: ErrorCode.NotFound, "Not found Rank with id =" + RankID);
            }

            _mapper.Map(UpdateRank, rank);
            //service.LastUpdatedBy = currentUserId;

            await genericRepository.UpdateAsync(rank);
            await genericRepository.SaveAsync();
        }
        public async Task<GetRankViewModel> GetByID(string id) {
			if (string.IsNullOrEmpty(id))
			{
				throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Rank id cannot be null or empty.");
			}
            else
            {
				var existedPets = await _unitOfWork.GetRepository<Rank>()
					 .Entities
					 .FirstOrDefaultAsync(p => p.Id == id && !p.DeletedTime.HasValue);
				if (existedPets == null)
				{
					throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Pet");
				}
                else
                {
                    return _mapper.Map<GetRankViewModel>(existedPets);
                } 
                    
			}
		}
    }
}

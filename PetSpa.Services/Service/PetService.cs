﻿using PetSpa.Contract.Repositories.Entity;
using Microsoft.AspNetCore.Http;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PetsModelViews;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Services.Interface;
using Microsoft.Extensions.Logging;

namespace PetSpa.Services.Service
{
    public class PetService : IPetService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(POSTPetsModelView petMV)
        {
            if (petMV == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Pet cannot be null.");
            }
            if (petMV.Name == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pet name is required.");
            }

            Pets pets = new Pets()
            {
                Name = petMV.Name,
                Species = petMV.Species,
                Weight = petMV.Weight,
                Breed = petMV.Breed,
                Age = petMV.Age,
                Description = petMV.Description,
                Image = petMV.Image,
                CreatedTime = DateTime.Now,

            };

            await _unitOfWork.GetRepository<Pets>().InsertAsync(pets);
            await _unitOfWork.SaveAsync();


        }

        public async Task Delete(string Id)
        {
            Pets? existedPet = await _unitOfWork.GetRepository<Pets>().GetByIdAsync(Id);
            if (existedPet == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Pet");
            }
            existedPet.DeletedTime = DateTime.Now;
            //existedPet.DeletedBy = currentUserId; // Sẽ thêm sau khi có authentication
            await _unitOfWork.GetRepository<Pets>().DeleteAsync(Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<GETPetsModelView>> GetAll(int pageNumber = 1, int pageSize = 2)
        {
            var pets = await _unitOfWork.GetRepository<Pets>().GetAllAsync();
            var PetsModelViews = pets.Select(r => new GETPetsModelView
            {
                Id = r.Id,
                Name = r.Name,
                Species = r.Species,
                Weight = r.Weight,
                Breed = r.Breed,
                Age = r.Age,
                Description = r.Description,
                Image = r.Image,

                CreatedTime = DateTime.Now,
            }).ToList();

            int totalPets = PetsModelViews.Count;

            var paginatedPets = PetsModelViews
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new BasePaginatedList<GETPetsModelView>(paginatedPets, totalPets, pageNumber, pageSize);
        }

        public async Task<GETPetsModelView?> GetById(string Id)
        {
            var existedPet = await _unitOfWork.GetRepository<Pets>()
                .Entities.FirstOrDefaultAsync(r => r.Id == Id);
            if (existedPet == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Pet");
            }

            return new GETPetsModelView
            {

                Id = existedPet.Id,
                Name = existedPet.Name,
                Species = existedPet.Species,
                Weight = existedPet.Weight,
                Breed = existedPet.Breed,
                Age = existedPet.Age,
                Description = existedPet.Description,
                Image = existedPet.Image,
                UserId = existedPet.Users.Id,
                CreatedTime = existedPet.CreatedTime,
            };
        }

        public async Task Update(PUTPetsModelView pet)
        {
            if (pet == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Pet cannot be null.");
            }

            Pets? existedPet = await _unitOfWork.GetRepository<Pets>().GetByIdAsync(pet.Id);
            if (existedPet == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Not found Pet");
            }

            if (string.IsNullOrWhiteSpace(pet.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Pet name is required.");
            }

            existedPet.Name = pet.Name;
            existedPet.Species = pet.Species;
            existedPet.Weight = pet.Weight;
            existedPet.Breed = pet.Breed;
            existedPet.Age = pet.Age;
            existedPet.Description = pet.Description;
            existedPet.Image = pet.Image;
            existedPet.LastUpdatedBy = pet.LastUpdatedBy;
            existedPet.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Pets>().UpdateAsync(existedPet);
            await _unitOfWork.SaveAsync();
        }
    }
}

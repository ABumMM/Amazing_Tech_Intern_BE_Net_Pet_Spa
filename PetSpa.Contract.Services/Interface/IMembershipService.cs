﻿using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.MemberShipModelView;
using PetSpa.ModelViews.MemberShipModelViews;
using PetSpa.ModelViews.PackageModelViews;
namespace PetSpa.Contract.Services.Interface
{
    public interface IMembershipsService
    {
        Task<BasePaginatedList<GETMemberShipModelView>> GetAll(int pageNumber, int pageSize);
        Task<GETMemberShipModelView?> GetById(string memberShipID);
        Task<List<GETMemberShipModelView?>> GetMemberShips(string packageID, DateTime? DateStart, DateTime? EndStart, string? Name);
        Task Add(POSTMemberShipModelView memberShipMV);
        Task Update(PUTMemberShipModelView memberShipMV);
        Task Delete(string id);
    }
}

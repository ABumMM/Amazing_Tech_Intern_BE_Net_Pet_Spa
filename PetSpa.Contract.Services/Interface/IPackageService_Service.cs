using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Base;
using PetSpa.ModelViews.PackageModelViews;
using PetSpa.ModelViews.PackageServiceModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IPackageService_Service
    {
        Task<BasePaginatedList<GETPackageServiceModelView>> GetAll(int pageNumber , int pageSize );
        Task<GETPackageServiceModelView?> GetById(string ID);
        Task<List<GETPackageModelView?>> GetPackages(string packageID, DateTime? DateStart, DateTime? EndStart, string? Name);
        Task Add(string packageID, string serviceID);
        Task Delete(string ID);
    }
}

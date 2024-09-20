using PetSpa.ModelViews.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IEmployeeService
    {
        Task<IList<EmployeeResponseModel>> GetAll();
        Task<EmployeeResponseModel?> GetById(object id);
        Task Add(EmployeeResponseModel emloyee);
        Task Update(EmployeeResponseModel emloyee);
        Task Delete(object id);
    }
}

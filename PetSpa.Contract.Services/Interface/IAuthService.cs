using PetSpa.ModelViews.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Contract.Services.Interface
{
    public interface IAuthService
    {
        Task Add(UserResponseModel user);
        Task Update(UserResponseModel user);
        Task Delete(object id);
    }
}

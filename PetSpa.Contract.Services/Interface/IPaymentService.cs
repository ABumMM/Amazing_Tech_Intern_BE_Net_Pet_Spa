using Microsoft.AspNetCore.Http;
using PetSpa.ModelViews.PaymentModelView;

namespace PetSpa.Contract.Services.Interface
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentUrl(HttpContext context, PostPaymentModelView payment);
        GetPaymentModelView PaymentExecute(IQueryCollection collection);
    }
}

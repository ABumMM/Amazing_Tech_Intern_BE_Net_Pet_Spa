using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.PaymentModelView;

namespace PetSpa.Services.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentService(IConfiguration config, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        public GetPaymentModelView PaymentExecute(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collection.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value.ToString();
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]!);
            if (!checkSignature)
            {
                return new GetPaymentModelView
                {
                    Successful = false,
                    OrderId = vnp_orderId.ToString(),
                    VnPayResponseCode = vnp_ResponseCode
                };
            }

            return new GetPaymentModelView
            {
                Successful = true,
                PaymentMethod = "VnPay",
                Description = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode,
            };
        }

        public async Task<string> CreatePaymentUrl(HttpContext context, PostPaymentModelView payment)
        {
            // Retrieve order by the provided OrderId
            var order = await _unitOfWork.GetRepository<Orders>().GetByIdAsync(payment.OrderId);
            if (order == null)
            {
                throw new Exception("Đơn hàng không tồn tại.");
            }

            // Fetch the FinalPrice from the order
            var finalPrice = order.FinalPrice;

            // Create payment request to VnPay
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]!);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]!);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]!);
            vnpay.AddRequestData("vnp_Amount", (finalPrice * 100).ToString()); // Final price from order
            vnpay.AddRequestData("vnp_CreateDate", CoreHelper.SystemTimeNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]!);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]!);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng: " + payment.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]!);
            vnpay.AddRequestData("vnp_ExpireDate", CoreHelper.SystemTimeNow.AddMinutes(5).ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_TxnRef", $"{tick}");

            await _unitOfWork.SaveAsync(); 
            var paymentUrl = vnpay.CreateRequestUrl(
                _config["PaymentSettings:BaseUrl"]!,
                _config["PaymentSettings:VnPayHashSecret"]!
            );


            return paymentUrl;
        }

    }
}

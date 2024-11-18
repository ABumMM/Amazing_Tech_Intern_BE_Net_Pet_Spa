using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services;
using PetSpa.Contract.Services.Interface;
using PetSpa.Repositories.UOW;
using PetSpa.Services.Service;

namespace PetSpa.Services
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddServices();
            services.AddRepositories();
            services.AddHttpContextAccessor();
            services.AddMapper();

        }
        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IMembershipsService, MemberShipService>();
            services.AddScoped<IBookingServicecs, BookingService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookingPackage_Service, BookingPackageService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IOrderDetailServices, OrderDetailService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IRankService,RankService>();

        }
    }

}
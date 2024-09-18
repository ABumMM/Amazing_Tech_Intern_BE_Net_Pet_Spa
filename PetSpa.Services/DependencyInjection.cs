using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Contract.Services.Interface;
using PetSpa.Repositories.UOW;
using PetSpa.Services.Service;

namespace PetSpa.Services
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddRepositories();
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

            services.AddScoped<IEmployeeService, EmployeeService>();
<<<<<<< HEAD
            services.AddScoped<IOrderService, OrderService>();
=======

<<<<<<< HEAD
>>>>>>> e83cad0dc99690be0e322fe4c65f3871def6d21e
=======

>>>>>>> 51b48a6c776267542752c9a63caeb0511a28142e
        }
    }

}
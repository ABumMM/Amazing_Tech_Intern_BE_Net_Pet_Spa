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


        }
    }

}
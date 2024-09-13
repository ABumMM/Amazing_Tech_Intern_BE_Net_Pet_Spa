using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
namespace PetSpa.Repositories.Context
{
    public class DatabaseContext:IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // user
        public virtual DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public virtual DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
        public virtual DbSet<ApplicationUserClaims> ApplicationUserClaims => Set<ApplicationUserClaims>();
        public virtual DbSet<ApplicationUserRoles> ApplicationUserRoles => Set<ApplicationUserRoles>();
        public virtual DbSet<ApplicationUserLogins> ApplicationUserLogins => Set<ApplicationUserLogins>();
        public virtual DbSet<ApplicationRoleClaims> ApplicationRoleClaims => Set<ApplicationRoleClaims>();
        public virtual DbSet<ApplicationUserTokens> ApplicationUserTokens => Set<ApplicationUserTokens>();
<<<<<<< HEAD
=======

>>>>>>> a1247fff7a39cde2f6ecd1b258c154411790114c
        public virtual DbSet<Bookings> Bookings { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<MemberShip> MemberShips { get; set; }
        public virtual DbSet<OrdersDetails> OrdersDetails { get; set; }
<<<<<<< HEAD
        public virtual DbSet<Orders> Orders { get; set; }
=======
        public virtual DbSet<Orders> Package { get; set; }
>>>>>>> a1247fff7a39cde2f6ecd1b258c154411790114c
        public virtual DbSet<Packages> Packages { get; set; }
        public virtual DbSet<Pets> Pets { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
<<<<<<< HEAD
=======
        //public virtual DbSet<UserInfo> UserInfos => Set<UserInfo>();

        /*public virtual DbSet<Orders> Orders => Set<Orders>();*/
>>>>>>> a1247fff7a39cde2f6ecd1b258c154411790114c
    }
}

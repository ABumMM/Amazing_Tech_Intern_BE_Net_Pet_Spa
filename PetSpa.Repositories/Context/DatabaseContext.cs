﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.PackageModelViews;
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
        public virtual DbSet<Bookings> Bookings { get; set; }
        public virtual DbSet<MemberShips> MemberShips { get; set; }
        public virtual DbSet<OrdersDetails> OrdersDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Packages> Packages { get; set; }
        public virtual DbSet<Pets> Pets { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ServicesEntity> Services { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<PackageServiceEntity> PackageServiceDTOs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

            // BOOKINGPACKAGE
            // Cấu hình khóa chính cho bảng trung gian BookingPackage
            modelBuilder.Entity<BookingPackage>()
                .HasKey(ps => new { ps.PackageId, ps.BookingId});
            // Cấu hình quan hệ giữa BookingPackage và Packages
            modelBuilder.Entity<BookingPackage>()
                .HasOne(ps => ps.Package)
                .WithMany(p => p.BookingPackages)
                .HasForeignKey(ps => ps.PackageId);
            // Cấu hình quan hệ giữa BookingPackage và Booking
            modelBuilder.Entity<BookingPackage>()
                .HasOne(ps => ps.Booking)
                .WithMany(s => s.BookingPackages)
                .HasForeignKey(ps => ps.BookingId);

            //ORDERDETAIL_PACKGAGE
            modelBuilder.Entity<OrdersDetails>()
                .HasMany(od => od.Packages)
                .WithOne(p => p.OrdersDetails) // Khóa ngoại trong Packages
                .HasForeignKey(p => p.OrderDetailID); // Đặt OrderDetailID là khóa ngoại
        }
    }
}

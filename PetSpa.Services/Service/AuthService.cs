﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Contract.Repositories.IUOW;
using PetSpa.Core.Base;
using PetSpa.ModelViews.AuthModelViews;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetSpa.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<string> SignUpAsync(SignUpAuthModelView signup)
        {
            if (signup == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "SignUp model cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(signup.Email) || !signup.Email.Contains("@"))
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Email is required and must be valid.");
            }
            if (string.IsNullOrWhiteSpace(signup.Password) || signup.Password.Length < 6)
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Password must be at least 6 characters long.");
            }

            ApplicationUser user = new ApplicationUser
            {
                UserInfo = new UserInfo { FullName = signup.FullName },
                Email = signup.Email,
                UserName = signup.Email,  // Thiết lập UserName rõ ràng
                //PhoneNumber = signup.PhoneNumber,
                Password = signup.Password,
            };

            var result = await _userManager.CreateAsync(user, signup.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    var role = new ApplicationRole { Name = "Admin" };
                    await _roleManager.CreateAsync(role);
                }

                result = await _userManager.AddToRoleAsync(user, "Admin");
                if (!result.Succeeded)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Error adding role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                return GenerateJwtToken(user);
            }

            throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Error occurred during signup: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }


        public async Task<string> SignInAsync(SignInAuthModelView signin)
        {
            if (signin == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "SignIn model cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(signin.Email) || !signin.Email.Contains("@"))
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Email is required and must be valid.");
            }
            if (string.IsNullOrWhiteSpace(signin.Password))
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Password cannot be empty.");
            }

            var user = await _userManager.FindByEmailAsync(signin.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, signin.Password))
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Invalid email or password.");
            }

            // Tạo token và trả về
            return GenerateJwtToken(user);
        }


        public async Task<bool> ChangePasswordAsync(ChangePasswordAuthModelView changepass, Guid userId)
        {
            if (changepass == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "ChangePassword model cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(changepass.CurrentPassword) || string.IsNullOrWhiteSpace(changepass.NewPassword))
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Current and new passwords cannot be empty.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, changepass.CurrentPassword, changepass.NewPassword);
            if (!result.Succeeded)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Error occurred while changing password: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return true;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("fullName", user.UserInfo?.FullName ?? string.Empty),
                    new Claim(ClaimTypes.Role, "")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

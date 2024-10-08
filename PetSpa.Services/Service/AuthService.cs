using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<string?> SignUpAsync(SignUpAuthModelView signup)
        {
            // Kiểm tra từng thuộc tính của signup
            if (signup.FullName == null)
            {
                throw new BadRequestException(ErrorCode.BadRequest, "Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(signup.Email) || !signup.Email.Contains("@"))
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Email is required and must be valid.");
            }

            if (string.IsNullOrWhiteSpace(signup.Password) || signup.Password.Length < 6)
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Password must be at least 6 characters long.");
            }

            var user = _mapper.Map<ApplicationUser>(signup);
            var result = await _userManager.CreateAsync(user, signup.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Customer"))
                {
                    var role = new ApplicationRole { Name = "Customer" };
                    await _roleManager.CreateAsync(role);
                }

                result = await _userManager.AddToRoleAsync(user, "Customer");
                if (!result.Succeeded)
                {
                    throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Error adding role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                return await GenerateJwtToken(user);
            }

            throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Error occurred during signup: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<string?> SignInAsync(SignInAuthModelView signin)
        {
            // Kiểm tra từng thuộc tính của signin
            if (signin.Email == null || !signin.Email.Contains("@"))
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
            return await GenerateJwtToken(user);
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordAuthModelView changepass, Guid userId)
        {
            // Kiểm tra từng thuộc tính của changepass
            if (changepass.CurrentPassword == null)
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "Current password cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(changepass.NewPassword))
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "New password cannot be empty.");
            }

            if (changepass.NewPassword.Length < 6)
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "New password must be at least 6 characters long.");
            }

            if (changepass.NewPassword != changepass.ConfirmPassword)
            {
                throw new BadRequestException(ErrorCode.InvalidInput, "New password and confirm password do not match.");
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

        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            // Lấy danh sách vai trò của người dùng
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("fullName", user.UserInfo?.FullName ?? string.Empty)
            };

            // Thêm các role claim vào danh sách claim
            claims.AddRange(roleClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

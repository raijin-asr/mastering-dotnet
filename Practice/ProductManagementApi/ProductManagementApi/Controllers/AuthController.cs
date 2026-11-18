

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.DTOs.Auth;
using ProductManagementApi.Models;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration; //it reads value of JWT settings store in appsettings.json and User Secrets.


        //Constructor
        public AuthController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var exisitingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (exisitingUser is not null)
            {
                return Conflict(new
                {
                    message = "Email is already registered."
                });
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };
            
            //this validates password, hashes it, store user and password hash
            var result = await _userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            //Prevents everyone being Admin, Now every successfully registered user gets the Customer role automatically.
            var roleResult = await _userManager.AddToRoleAsync(
                    user, "Customer");

            if (!roleResult.Succeeded)
            {
                return StatusCode(500, new
                {
                    message = "User was created, but assigning the Customer role failed."
                });
            }

            return StatusCode(201, new
            {
                message = "User registered successfully."
            });
           
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // 1. Find user by email
            var user = await _userManager.FindByEmailAsync(dto.Email);

            // 2. Check whether user exists and password is correct
            if (user == null ||
                !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            // 3. Read JWT Configuration
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key is missing.");

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(issuer) ||
                string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException("JWT Issuer or Audience is missing.");
            }

            var expirationMinutes =
                _configuration.GetValue<int>("Jwt:ExpiresInMinutes", 60);

            //4. Get all roles assigned to logged-in users
            var roles = await _userManager.GetRolesAsync(user);

            // 5. Create claims describing the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id), // user ID is added to JWT to later use it without Query
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Add roles to Claim
            claims.AddRange(
                roles.Select(role => new Claim(ClaimTypes.Role, role))
                );

            //Add Permision ManagePrices for Policy here
            if (roles.Contains("Admin"))
            {
                claims.Add(
                    new Claim("Permission", "ManagePrices")
                    );
            }

            // 6. Create signing credentials using our secret key
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            // 6. Create the JWT
            var expires = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            // 7. Convert JWT object into a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 8. Return token to client
            return Ok(new
            {
                token = tokenString,
                expiration = expires,
                email = user.Email
            });
        }

        [Authorize]
        [HttpGet("LoggedInUser")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            return Ok(new
            {
                userId,
                email
            });
        }
    }
}

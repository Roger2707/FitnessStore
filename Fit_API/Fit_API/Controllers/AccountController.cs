using Fit_API.DataAccess.Data;
using Fit_API.DataAccess.Services;
using Fit_API.Extensions;
using Fit_API.Models;
using Fit_API.Models.DTOs;
using Fit_API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseAPIController
    {
        private readonly ApplicationDbContext _db;
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private TokenService _tokenService;
        public AccountController(ApplicationDbContext db, UserManager<User> userManager, RoleManager<Role> roleManager, TokenService tokenService)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.UserName);
            if(user == null || ! await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return Unauthorized();
            }

            var existedCart = await _db.ShoppingCarts.RetrieveShoppingCart(loginDTO.UserName).FirstOrDefaultAsync();
            var newCart = new ShoppingCart() { UserId = loginDTO.UserName };

            if(existedCart == null)
            {
                await _db.ShoppingCarts.AddAsync(newCart);
                await _db.SaveChangesAsync();
            }

            return new UserDTO
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                ShoppingCart = existedCart != null ? existedCart.MapShoppingCartToDTO() : newCart.MapShoppingCartToDTO(),
            };
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var user = new User
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(201);
        }

        [HttpGet("currentuser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> CurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var existedCart = await _db.ShoppingCarts.RetrieveShoppingCart(User.Identity.Name).FirstOrDefaultAsync();

            return new UserDTO
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                ShoppingCart = existedCart.MapShoppingCartToDTO(),
            };
        }

        
    }
}

using Fit_API.DataAccess.Data;
using Fit_API.Extensions;
using Fit_API.Models;
using Fit_API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fit_API.Controllers
{
    public class ShoppingCartController : BaseAPIController
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "GetShoppingCart")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartDTO>> GetShoppingCart()
        {
            var shoppingCart = await _db.ShoppingCarts
                                        .RetrieveShoppingCart(User.Identity?.Name)
                                        .FirstOrDefaultAsync();

            if (shoppingCart == null) return BadRequest(new ProblemDetails { Title = "Can not retrieve shopping cart !" });

            return shoppingCart.MapShoppingCartToDTO();
        }

        [HttpPost("addremoveitem")]
        [Authorize]
        public async Task<ActionResult<ShoppingCartDTO>> AddRemoveItemInCart(int productId, int quantity, string type)
        {
            // Get Shopping Cart

            var shoppingCart = await _db.ShoppingCarts
                                        .RetrieveShoppingCart(User.Identity?.Name)
                                        .FirstOrDefaultAsync();

            if (shoppingCart == null) return BadRequest(new ProblemDetails { Title = "Can not retrieve shopping cart !" });

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return NotFound();

            if(type == "add")
            {
                shoppingCart.AddItem(product, quantity);
            } 
            else if(type == "remove")
            {
                shoppingCart.RemoveItem(productId, quantity);
            }

            var result = await _db.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetShoppingCart", shoppingCart.MapShoppingCartToDTO());

            return BadRequest(new ProblemDetails { Title = "Problem try to add to shopping cart ! Try Again !!!" });
        }
    }
}
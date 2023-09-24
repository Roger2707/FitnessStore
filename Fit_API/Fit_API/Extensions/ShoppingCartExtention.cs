using Fit_API.Models;
using Fit_API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Fit_API.Extensions
{
    public static class ShoppingCartExtention
    {
        public static IQueryable<ShoppingCart> RetrieveShoppingCart(this IQueryable<ShoppingCart> query, string userId)
        {
            return query.Where(x => x.UserId == userId)
                        .Include(c => c.Items)
                        .ThenInclude(i => i.Product).ThenInclude(p => p.ProductInstructors).ThenInclude(p => p.Instructor);
        }

        public static ShoppingCartDTO MapShoppingCartToDTO(this ShoppingCart shoppingCart)
        {
            return new ShoppingCartDTO
            {
                Id = shoppingCart.Id,
                UserId = shoppingCart.UserId,
                Items = shoppingCart.Items.Select(i => new CartItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ShoppingCartId = i.ShoppingCartId,
                    Name = i.Product.Name,
                    Description = i.Product.Description,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity,
                    Price = i.Product.Price * i.Quantity,
                    InstructorsName = i.Product.ProductInstructors.Select(i => i.Instructor.Name).ToList(),
                }).ToList(),
            };
        }
    }
}

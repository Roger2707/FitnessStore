using Fit_API.DataAccess.Data;
using Fit_API.Extensions;
using Fit_API.Models.DTOs;
using Fit_API.Models.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fit_API.Controllers
{
    [Authorize]
    public class OrdersController : BaseAPIController
    {
        private readonly ApplicationDbContext _db;
        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            var orders = await _db.Orders
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.Status.ToString(),
                    ShippingAddress = o.ShippingAddress,
                    DeliveryFee = o.DeliveryFee,
                    SubTotal = o.Subtotal,
                    Total = o.GetTotal(),
                    OrderItemsDTO = o.Items.Select(o => new OrderItemDTO
                    {
                        ProductId = o.ProductItemOrdered.ProductId,
                        Name = o.ProductItemOrdered.Name,
                        PictureUrl = o.ProductItemOrdered.PictureUrl,
                        Price = o.Price,
                        Quantity = o.Quantity,
                    }).ToList(),
                }).AsNoTracking()
                .Where(x => x.UserId == User.Identity.Name)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id:int}", Name = "GetOrder")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrder(int? id)
        {
            var order = await _db.Orders
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.Status.ToString(),
                    ShippingAddress = o.ShippingAddress,
                    DeliveryFee = o.DeliveryFee,
                    SubTotal = o.Subtotal,
                    Total = o.GetTotal(),
                    OrderItemsDTO = o.Items.Select(o => new OrderItemDTO
                    {
                        ProductId = o.ProductItemOrdered.ProductId,
                        Name = o.ProductItemOrdered.Name,
                        PictureUrl = o.ProductItemOrdered.PictureUrl,
                        Price = o.Price,
                        Quantity = o.Quantity,
                    }).ToList(),
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == User.Identity.Name && x.Id == id);

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDTO>> CreateOrder(OrderCreateDTO orderCreate)
        {
            var shoppingcart = await _db.ShoppingCarts.RetrieveShoppingCart(User.Identity.Name).FirstOrDefaultAsync();

            if (shoppingcart == null) return BadRequest(new ProblemDetails { Title = "Shopping Cart is NULL !" });

            var listOrderItem = new List<OrderItem>();

            foreach(var cartItem in shoppingcart.Items)
            {
                var product = await _db.Products.FindAsync(cartItem.ProductId);

                var productItemOrdered = new ProductItemOrdered
                {
                    ProductId = cartItem.ProductId,
                    Name = cartItem.Product.Name,
                    PictureUrl = cartItem.Product.ImageUrl,
                };

                var orderItem = new OrderItem()
                {
                    ProductItemOrdered = productItemOrdered,
                    Quantity = cartItem.Quantity,
                    Price = product.Price,
                };

                listOrderItem.Add(orderItem);
            }

            var subTotal = listOrderItem.Sum(o => o.Quantity*o.Price);
            var deliveryFee = subTotal > 1000 ? 0 : 50;

            // Create
            var order = new Order()
            {
                UserId = User.Identity.Name,
                ShippingAddress = orderCreate.ShippingAddress,
                DeliveryFee = deliveryFee,
                Subtotal = subTotal,
                Items = listOrderItem,
            };

            _db.Orders.Add(order);
            _db.ShoppingCarts.Remove(shoppingcart);

            // Save Address 

            if(orderCreate.SaveAddress)
            {
                var user = await _db.Users.Include(u => u.Address).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                var address = new UserAddress()
                {
                    FullName = orderCreate.ShippingAddress.FullName,
                    Address1 = orderCreate.ShippingAddress.Address1,
                    Address2 = orderCreate.ShippingAddress.Address2,
                    City = orderCreate.ShippingAddress.City,
                    State = orderCreate.ShippingAddress.State,
                    Zip = orderCreate.ShippingAddress.Zip,
                    Country = orderCreate.ShippingAddress.Country
                };

                user.Address = address;
            }

            var result = await _db.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);

            return BadRequest("Problem Creating Order !");
        }
    }
}

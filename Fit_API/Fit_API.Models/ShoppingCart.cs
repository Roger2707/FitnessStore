using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fit_API.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public void AddItem(Product product, int quantity)
        {
            var existedItem = Items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (existedItem == null) Items.Add(new CartItem { ProductId = product.Id, Product = product, Quantity = quantity });
            else existedItem.Quantity += quantity;
        }

        public void RemoveItem(int productId, int quantity)
        {
            var existedItem = Items.FirstOrDefault(x => x.ProductId == productId);
            if (existedItem == null) return;
            existedItem.Quantity -= quantity;
            if(existedItem.Quantity == 0) Items.Remove(existedItem);
        }
    }
}

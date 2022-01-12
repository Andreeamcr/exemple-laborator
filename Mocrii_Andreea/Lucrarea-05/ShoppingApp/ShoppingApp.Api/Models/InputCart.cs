using System.ComponentModel.DataAnnotations;
using ShoppingApp.Domain.Models;

namespace ShoppingApp.Api.Models
{
    public class InputCart
    {
        [Required]
        [RegularExpression(ProductCode.ValidPattern)]
        public string Code { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Range(0, 999999999)]
        public decimal Price { get; set; }
    }
}

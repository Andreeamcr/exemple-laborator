

using ShoppingApp.Dto.Models;

namespace ShoppingApp.Dto
{
    public class CartPublishedEvent
    {
        public List<ProductDto> Products { get; init; }
    }
}
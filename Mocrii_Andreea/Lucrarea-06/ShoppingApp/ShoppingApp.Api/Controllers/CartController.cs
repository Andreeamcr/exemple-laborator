using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ShoppingApp.Domain.Repositories;
using System.Linq;
using ShoppingApp.Domain;
using ShoppingApp.Api.Models;
using ShoppingApp.Domain.Models;

namespace ShoppingApp.Api.Controllers
{

    [ApiController]
    [Route("controller")]
    public class CartController : ControllerBase
    {
        private ILogger<CartController> logger;

        public CartController(ILogger<CartController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromServices] IOrderLinesRepository orderLinesRepository ) =>
            await orderLinesRepository.TryGetExistingOrderLines().Match(
                Succ: GetAllProductsHandleSuccess,
                Fail: GetAllProductsHandleError
            );

        private ObjectResult GetAllProductsHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }

        private OkObjectResult GetAllProductsHandleSuccess(List<ShoppingApp.Domain.Models.CalculatedProduct> calculatedProducts) =>
        Ok(calculatedProducts.Select(product => new
        {
            Code = product.Code.Value,
            product.Price,
            product.Quantity,
            product.DestinationAddress,
            product.TotalPrice
        }));

        [HttpPost]
        public async Task<IActionResult> PublishCart([FromServices] CartWorkflow cartWorkflow, [FromBody] InputCart[] products)
        {
            var unvalidatedProducts = products.Select(MapInputCartToUnvalidatedProduct)
                                              .ToList()
                                              .AsReadOnly();
            CartCommand command = new(unvalidatedProducts);
            var result = await cartWorkflow.ExecuteAsync(command);
            return result.Match<IActionResult>(
                whenFailedPaidCartEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                whenSuccessPaidCartEvent: successEvent => Ok()
            );
        }

        private static UnvalidatedProduct MapInputCartToUnvalidatedProduct(InputCart product) => new UnvalidatedProduct(
            ProductCode: product.Code,
            Quantity: product.Quantity.ToString(),
            Price: product.Price.ToString(),
            DestinationAddress: product.Address
            );
    }
}

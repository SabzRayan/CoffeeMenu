using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Products;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductParams param)
        {
            return HandlePagedResult(await Mediator.Send(new ListProduct.Query { Params = param }));
        }

        [AllowAnonymous]
        [HttpGet("best")]
        public async Task<IActionResult> GetBestProducts([FromQuery] ProductParams param)
        {
            return HandleResult(await Mediator.Send(new ListBestProduct.Query { Params = param }));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new ProductDetails.Query { Id = id }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            return HandleResult(await Mediator.Send(new CreateProduct.Command { Product = product }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditProduct([FromRoute] Guid id, [FromBody] Product product)
        {
            product.Id = id;
            return HandleResult(await Mediator.Send(new EditProduct.Command { Product = product }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPut("{id}/setAvailable")]
        public async Task<IActionResult> SetAvailable([FromRoute] Guid id, [FromBody] bool isAvailable)
        {
            return HandleResult(await Mediator.Send(new SetAvailableProduct.Command { Id = id, IsAvailable = isAvailable }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpPut("{id}/setExist")]
        public async Task<IActionResult> SetExist([FromRoute] Guid id, [FromBody] bool isExist)
        {
            return HandleResult(await Mediator.Send(new SetExistProduct.Command { Id = id, IsExist = isExist }));
        }

        [Authorize(Policy = "IsManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteProduct.Command { Id = id }));
        }
    }
}

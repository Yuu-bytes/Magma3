using Magma3.Application.Products.Commands.CreateProduct;
using Magma3.Application.Products.Commands.UpdateProduct;
using Magma3.Application.Products.Commands.DeleteProduct;
using Magma3.Application.Products.Queries.GetProduct;
using Magma3.Application.Products.Queries.GetProducts;
using Magma3.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Magma3.Controllers
{
    public class ProductsController : ApiBaseController
    {
        /// <summary>
        /// Retrieves all products
        /// </summary>
        /// <returns>Returns a list of products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetProductsQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific product by ID
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>Returns the product</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetProductQuery { Id = id };
            var result = await Mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="command">The product data</param>
        /// <returns>Returns the created product</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <param name="command">The updated product data</param>
        /// <returns>Returns the updated product</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await Mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>Returns no content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await Mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

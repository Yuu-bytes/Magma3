using Magma3.Application.Clientes.Commands.CreateCliente;
using Magma3.Application.Clientes.Commands.UpdateCliente;
using Magma3.Application.Clientes.Commands.DeleteCliente;
using Magma3.Application.Clientes.Queries.GetCliente;
using Magma3.Application.Clientes.Queries.GetClientes;
using Magma3.Application.DTOs.Clientes;
using Microsoft.AspNetCore.Mvc;

namespace Magma3.Controllers
{
    public class ClientesController : ApiBaseController
    {
        /// <summary>
        /// Retrieves all clientes
        /// </summary>
        /// <returns>Returns a list of clientes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClienteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetClientesQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific cliente by ID
        /// </summary>
        /// <param name="id">The cliente ID</param>
        /// <returns>Returns the cliente</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetClienteQuery { Id = id };
            var result = await Mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new cliente
        /// </summary>
        /// <param name="command">The cliente data</param>
        /// <returns>Returns the created cliente</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Create([FromBody] CreateClienteCommand command)
        {
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing cliente
        /// </summary>
        /// <param name="id">The cliente ID</param>
        /// <param name="command">The updated cliente data</param>
        /// <returns>Returns the updated cliente</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClienteCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a cliente
        /// </summary>
        /// <param name="id">The cliente ID</param>
        /// <returns>Returns no content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteClienteCommand { Id = id };
            var result = await Mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

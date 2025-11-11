using Magma3.Application.Assets.Queries.GetAtivosQuery;
using Magma3.Application.Assets.Queries.GetComputersOff;
using Magma3.WebClient.WebClient.Response;
using Microsoft.AspNetCore.Mvc;

namespace Magma3.Controllers
{
    public class AssetsController : ApiBaseController
    {
        /// <summary>
        /// Retrieves assets from the Magma3 system
        /// </summary>
        /// <returns>Returns a list of assets</returns>
        [HttpGet("ComputersOff")]
        [ProducesResponseType(typeof(GetAssetsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetComputersOff()
        {
            var query = AuthorizationRequestCreate<GetComputersOffQuery>();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAtivos")]
        /// <summary>
        /// Get a list of ativos
        /// </summary>
        /// <param name="page">Page</param>
        [ProducesResponseType(typeof(List<GetAssetsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> GetAtivos([FromHeader] int page)
        {
            var query = new GetAtivosQuery() { Page = page };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        //public async Task<Ativo> PegaAtivos()
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    var resposta = await client.GetAsync($"https://api.magma-3.com/v2/Force1/GetAssets");

        //    resposta.EnsureSuccessStatusCode();

        //    var conteudo = await resposta.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<Ativo>(conteudo);
        //}
    }
}

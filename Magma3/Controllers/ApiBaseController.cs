using Magma3.Application;
using Magma3.Application.Common.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Magma3.Controllers
{
    [ApiKey]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ApiBaseController : ControllerBase
    {
        private ISender _mediator = null!;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected T AuthorizationRequestCreate<T>() where T : AuthorizationBaseRequest, new()
        {

            if (HttpContext.Request.Headers.TryGetValue("AccessToken", out var name))
            {
                try
                {
                    Guid accessToken = new Guid(name!);
                    return new T { AccessToken = accessToken };
                }
                catch
                {
                    return new T(); ;
                }
            }
            else
            {
                return new T(); ;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Magma3.Application.Common.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "ApiKey was not provided"
                };
                return;
            }
            Guid accesstoken;
            try
            {
                accesstoken = new Guid(extractedApiKey!);
            }
            catch
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "ApiKey is not Valid"
                };
                return;
            }

            Guid? userApiKey = null; // var aqui normalmente procuraria o ApiKey no banco de dados

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            Guid masterToken = new Guid(configuration["TokenMaster"]!);

            if (userApiKey == null && masterToken != accesstoken)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "ApiKey is not valid"
                };
                return;
            }

            foreach (var command in context.ActionArguments)
            {
                if (command.Value != null)
                {

                    var authorization = command.Value as AuthorizationBaseRequest;
                    if (authorization != null)
                    {
                        authorization.AccessToken = userApiKey is null ? masterToken : userApiKey.Value;
                        break;
                    }
                }
            }

            await next();
        }
    }
}

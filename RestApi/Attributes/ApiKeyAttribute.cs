using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace RestApi.Attributes
{
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //var todoId = (Guid)context.ActionArguments["Id"];
            var key = context.HttpContext.Request.Headers["ApiKey"].SingleOrDefault();

            if (string.IsNullOrWhiteSpace(key))
            {
                context.Result = new BadRequestObjectResult("ApiKey header is missing");

                return;
            }

            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var apiKey = await userRepository.GetApiKeyAsync(key);

            if (apiKey is null)
            {
                context.Result = new NotFoundObjectResult("ApiKey is not found");

                return;
            }

            if (!apiKey.IsActive)
            {
                context.Result = new BadRequestObjectResult("ApiKey expired");

                return;
            }

            context.HttpContext.Items.Add("userId", apiKey.UserId);

            await next();
        }
    }
}

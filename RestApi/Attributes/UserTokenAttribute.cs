using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace RestApi.Attributes
{
    public class UserTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers["UserToken"].SingleOrDefault();

            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new BadRequestObjectResult("UserToken header is missing");

                return;
            }

            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            var userToken = await userRepository.GetUserTokenAsync(token);

            if (userToken is null)
            {
                context.Result = new NotFoundObjectResult("UserToken is not found");

                return;
            }

            var currentDate = DateTime.Now;

            if (userToken.ExpirationDate < currentDate)
            {
                context.Result = new BadRequestObjectResult("UserToken expired");

                return;
            }

            context.HttpContext.Items.Add("userId", userToken.UserId);

            await next();
        }
    }
}

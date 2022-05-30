using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Text;

namespace TechDrum.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SessionLifeTimeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(context.HttpContext.Session.GetString("JWToken")) || string.IsNullOrEmpty(context.HttpContext.Session.GetString("refreshToken")))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Auth",
                    action = "SignIn"
                }));
            }
        }
    }
}

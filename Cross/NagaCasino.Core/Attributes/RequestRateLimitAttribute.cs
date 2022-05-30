using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace TechDrum.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestRateLimitAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }

        public int Seconds { get; set; }

        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ipAddress = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;

            var memoryCacheKey = $"{Name}-{ipAddress}";

            if (!Cache.TryGetValue(memoryCacheKey, out bool _))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(Seconds));

                Cache.Set(memoryCacheKey, true, cacheEntryOptions);
            }
            else
            {
                context.Result = new RedirectResult("Oops/429");
            }
        }
    }
}
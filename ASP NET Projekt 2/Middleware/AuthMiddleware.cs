using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt_2
{
    public class AuthMiddleware
    {
        private RequestDelegate next;
        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            
            //string userAgent = ctx.Request.Headers["User-Agent"];
            //if(userAgent.Contains("Chrome"))
            //{
            //    await this.next.Invoke(ctx);
            //    return;
            //}

            ctx.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            await ctx.Response.WriteAsync("Použíte Chrome....");
            
        }
    }
}

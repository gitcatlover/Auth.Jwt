using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Core.AuthHelper
{
    public class JwtTokenAuth
    {
        private readonly RequestDelegate _next;
        public JwtTokenAuth(RequestDelegate next)
        {
            this._next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            string tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
            try
            {
                if (tokenHeader.Length >= 128)
                {
                    TokenModelJwt tokenModel = JwtHelper.SerializeJwt(tokenHeader);
                    //授权Claim
                    List<Claim> claims = new List<Claim>();
                    Claim claim = new Claim(ClaimTypes.Role, tokenModel.Role);
                    claims.Add(claim);
                    ClaimsIdentity identity = new ClaimsIdentity(claims);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    httpContext.User = principal;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} middleware wrong:{ex.Message}");
            }
            return _next(httpContext);
        }
    }

    //定义一个中间件Helper,给当前中间件模块取一个别名
    public static class MiddlewareHelpers
    {
        public static IApplicationBuilder UseJwtTokenAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtTokenAuth>();
        }
    }
}

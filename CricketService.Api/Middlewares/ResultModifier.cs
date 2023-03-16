using System.Buffers;
using Newtonsoft.Json;

namespace CricketService.Api.Middlewares
{
    public class ResultModifier
    {
        private readonly RequestDelegate next;

        public ResultModifier(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("copyright-company", "cricket-dekho");

            context.Response.Headers.Add("time-stamp", DateTime.Now.ToString());

            await next(context);
        }
    }
}

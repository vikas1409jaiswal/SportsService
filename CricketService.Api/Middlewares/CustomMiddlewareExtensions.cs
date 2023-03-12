namespace CricketService.Api.Middlewares
{

    public static class CustomMiddlewareExtensions
    {
       public static void UseResultModifier(this IApplicationBuilder app)
        {
            app.UseMiddleware<ResultModifier>();
        }
    }
}

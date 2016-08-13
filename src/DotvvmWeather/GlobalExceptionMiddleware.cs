using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace DotvvmWeather
{
    public class GlobalExceptionMiddleware : OwinMiddleware
    {
        public GlobalExceptionMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("HTTP Request Exception: " + ex.ToString());
            }
        }
    }
}
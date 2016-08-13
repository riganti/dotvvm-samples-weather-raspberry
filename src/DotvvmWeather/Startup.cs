using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using DotVVM.Framework.Hosting;
using Microsoft.Owin.Security.DataProtection;

[assembly: OwinStartup(typeof(DotvvmWeather.Startup))]
namespace DotvvmWeather
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.SetDataProtectionProvider(new MonoDataProtectionProvider());

            appBuilder.Use<GlobalExceptionMiddleware>();

            // DotVVM initialization
            var applicationPhysicalPath = Program.ApplicationDirectory;
            var dotvvmConfiguration = appBuilder.UseDotVVM<DotvvmStartup>(applicationPhysicalPath);

            appBuilder.UseFileServer(new FileServerOptions()
            {
                RequestPath = new PathString("/Scripts"),
                FileSystem = new PhysicalFileSystem(@"./Scripts")
            });

            appBuilder.UseFileServer(new FileServerOptions()
            {
                RequestPath = new PathString("/fonts"),
                FileSystem = new PhysicalFileSystem(@"./fonts"),
            });

            appBuilder.UseFileServer(new FileServerOptions()
            {
                RequestPath = new PathString("/Content"),
                FileSystem = new PhysicalFileSystem(@"./Content"),
            });

            appBuilder.Run(context =>
            {
                try
                {
                    return Task.FromResult(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in /: " + ex.Message);
                    return Task.FromResult(0);
                }
            });

        }
    }
}

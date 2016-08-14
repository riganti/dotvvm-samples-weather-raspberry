using System;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;

namespace DotvvmWeather
{
    public class HostingService : ServiceBase
    {
        private IDisposable server;

        protected override void OnStart(string[] args)
        {
            server = WebApp.Start<Startup>(Program.BaseUrl);

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            server.Dispose();

            base.OnStop();
        }
    }
}
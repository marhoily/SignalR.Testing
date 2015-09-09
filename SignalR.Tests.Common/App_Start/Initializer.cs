using System.Web;
using Microsoft.AspNet.SignalR.Tests.Common;
using Microsoft.Owin;
using Owin;

[assembly: PreApplicationStartMethod(typeof (Initializer), "Start")]
[assembly: OwinStartup(typeof (Initializer))]

namespace Microsoft.AspNet.SignalR.Tests.Common
{
    public static class Initializer
    {
        public static void ConfigureRoutes(IAppBuilder app, IDependencyResolver resolver)
        {
            var hubConfig = new HubConfiguration
            {
                Resolver = resolver,
                EnableDetailedErrors = true
            };

            app.MapSignalR(hubConfig);

            // IMPORTANT: This needs to run last so that it runs in the "default" part of the pipeline

            // Session is enabled for ASP.NET on the session path
            app.Map("/session", map => { map.MapSignalR(); });
        }
    }
}
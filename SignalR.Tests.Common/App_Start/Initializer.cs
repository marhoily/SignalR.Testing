using Microsoft.AspNet.SignalR;
using Owin;

namespace SignalR.Tests.Common
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

        }
    }
}
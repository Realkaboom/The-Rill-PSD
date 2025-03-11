using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(JAwelsAndDiamonds.Startup))]

namespace JAwelsAndDiamonds
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Views/Auth/Login.aspx"),
                ExpireTimeSpan = TimeSpan.FromDays(14), // Cookie valid for 14 days
                SlidingExpiration = true
            });

        }
    }
}
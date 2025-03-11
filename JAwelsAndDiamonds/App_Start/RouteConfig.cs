using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace JAwelsAndDiamonds
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            if (!RouteTable.Routes.Contains(new Route("AspNet.FriendlyUrls.SwitchView", null)))
            {
                var settings = new FriendlyUrlSettings();
                settings.AutoRedirectMode = RedirectMode.Permanent;
                routes.EnableFriendlyUrls(settings);
            }
        }
    }
}

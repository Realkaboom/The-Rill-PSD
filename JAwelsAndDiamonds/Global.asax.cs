using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace JAwelsAndDiamonds
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Register any global filters here if needed
            // GlobalFilters.Add(new MyCustomFilter());

            // Configure any database initializers here if needed
            // Database.SetInitializer<JAwelsAndDiamondsEntities>(new CreateDatabaseIfNotExists<JAwelsAndDiamondsEntities>());
        }

        void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();

            if (ex != null)
            {
                // Log the error - you can implement a more robust logging mechanism
                System.Diagnostics.Debug.WriteLine($"Unhandled Exception: {ex.Message}");

                // Redirect to error page
                if (HttpContext.Current != null)
                {
                    Server.ClearError();
                    Response.Redirect("~/Error.aspx");
                }
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends
        }
    }
}
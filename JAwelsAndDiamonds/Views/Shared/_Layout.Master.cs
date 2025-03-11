using System;
using System.Web.UI;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Shared
{
    public partial class _Layout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNavigation();
            }
        }

        private void LoadNavigation()
        {
            // Check if the user is logged in by checking the session
            object role = SessionUtil.GetSession(Session, "Role");

            if (role == null)
            {
                // User is not logged in, load guest navigation
                Control guestNav = LoadControl("~/Views/Shared/_GuestNavigation.ascx");
                NavigationContent.Controls.Add(guestNav);
            }
            else if (role.ToString() == "Admin")
            {
                // User is an admin, load admin navigation
                Control adminNav = LoadControl("~/Views/Shared/_AdminNavigation.ascx");
                NavigationContent.Controls.Add(adminNav);
            }
            else if (role.ToString() == "Customer")
            {
                // User is a customer, load customer navigation
                Control customerNav = LoadControl("~/Views/Shared/_CustomerNavigation.ascx");
                NavigationContent.Controls.Add(customerNav);
            }
        }
    }
}
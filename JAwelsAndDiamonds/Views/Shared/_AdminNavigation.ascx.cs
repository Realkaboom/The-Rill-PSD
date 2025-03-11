using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JAwelsAndDiamonds.Views.Shared
{
	public partial class _AdminNavigation : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Hapus semua data session
            Session.Clear();
            Session.Abandon();

            // Hapus cookie autentikasi jika ada
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            // Redirect ke halaman utama
            Response.Redirect("~/Views/Home/Index.aspx");
        }
    }
}
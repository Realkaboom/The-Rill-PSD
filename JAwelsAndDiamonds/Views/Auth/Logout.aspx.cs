using System;
using System.Web;

namespace JAwelsAndDiamonds.Views.Auth
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Hapus semua data session
                Session.Clear();
                Session.Abandon();

                // Hapus cookie autentikasi jika ada
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                }

                // Hapus cookie otentikasi OWIN jika menggunakan OWIN
                HttpCookie cookie = new HttpCookie(".AspNet.ApplicationCookie");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);

                // Redirect sudah ditangani oleh meta refresh di halaman HTML
                // Jika Anda ingin redirect secara langsung, bisa menggunakan:
                // Response.Redirect("~/Views/Home/Index.aspx");
            }
        }
    }
}
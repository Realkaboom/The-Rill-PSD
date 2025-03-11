using System;
using System.Web;
using System.Web.Security;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Auth
{
    public partial class Login : System.Web.UI.Page
    {
        private AuthController _authController;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is already logged in
            if (SessionUtil.GetSession(Session, "UserId") != null)
            {
                // Redirect to home page
                Response.Redirect("~/Views/Home/Index.aspx");
                return;
            }

            // Initialize the auth controller
            JAwelsAndDiamondsEntities context = new JAwelsAndDiamondsEntities();
            IUserRepository userRepository = new UserRepository(context);
            UserFactory userFactory = new UserFactory();
            AuthHandler authHandler = new AuthHandler(userRepository, userFactory);
            _authController = new AuthController(authHandler, this);

            if (!IsPostBack)
            {
                // Check if there's a remember me cookie
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    try
                    {
                        // Decrypt the cookie
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                        if (ticket != null && !ticket.Expired)
                        {
                            // Get user data
                            string email = ticket.Name;
                            int userId = int.Parse(ticket.UserData);

                            // Auto login
                            User user = userRepository.GetById(userId);
                            if (user != null && user.Email == email)
                            {
                                // Set session variables
                                SessionUtil.SetSession(Session, "UserId", user.UserId);
                                SessionUtil.SetSession(Session, "Username", user.Username);
                                SessionUtil.SetSession(Session, "Role", user.Role);

                                // Redirect to home page
                                Response.Redirect("~/Views/Home/Index.aspx");
                            }
                        }
                    }
                    catch
                    {
                        // Ignore and continue to login page
                    }
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            bool rememberMe = chkRememberMe.Checked;

            string errorMessage;
            bool success = _authController.Login(email, password, rememberMe, out errorMessage);

            if (!success)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Redirect to home page
            Response.Redirect("~/Views/Home/Index.aspx");
        }
    }
}
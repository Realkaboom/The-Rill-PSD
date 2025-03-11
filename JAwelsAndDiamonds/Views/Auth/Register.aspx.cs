using System;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Auth
{
    public partial class Register : System.Web.UI.Page
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
                // Set maximum date for date of birth
                dpDateOfBirth.Attributes["max"] = new DateTime(2010, 1, 1).ToString("yyyy-MM-dd");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string gender = rbMale.Checked ? "Male" : "Female";

            DateTime dateOfBirth;
            if (!DateTime.TryParse(dpDateOfBirth.Text, out dateOfBirth))
            {
                // Show error message for invalid date
                pnlAlert.Visible = true;
                ltError.Text = "Invalid date of birth.";
                return;
            }

            string errorMessage;
            bool success = _authController.Register(email, username, password, confirmPassword, gender, dateOfBirth, out errorMessage);

            if (!success)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Redirect to login page with success message
            Response.Redirect("Login.aspx?registered=true");
        }
    }
}
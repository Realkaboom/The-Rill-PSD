using System;
using JAwelsAndDiamonds.Controllers;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Views.Profile
{
    public partial class Profile : System.Web.UI.Page
    {
        private UserController _userController;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            object userId = SessionUtil.GetSession(Session, "UserId");
            if (userId == null)
            {
                // Redirect to login page
                Response.Redirect("~/Views/Auth/Login.aspx");
                return;
            }

            // Initialize user controller
            JAwelsAndDiamondsEntities context = new JAwelsAndDiamondsEntities();
            IUserRepository userRepository = new UserRepository(context);
            UserFactory userFactory = new UserFactory();
            UserHandler userHandler = new UserHandler(userRepository, userFactory);
            _userController = new UserController(userHandler, this);

            if (!IsPostBack)
            {
                // Load user profile
                LoadUserProfile();
            }
        }

        private void LoadUserProfile()
        {
            // Get user ID from session
            int userId = (int)SessionUtil.GetSession(Session, "UserId");

            // Get user details
            User user = _userController.GetUserById(userId);
            if (user == null)
            {
                // User not found, redirect to login page
                Response.Redirect("~/Views/Auth/Login.aspx");
                return;
            }

            // Set user details
            ltEmail.Text = user.Email;
            ltUsername.Text = user.Username;
            ltGender.Text = user.Gender;
            ltDateOfBirth.Text = user.DateOfBirth.ToString("MM/dd/yyyy");
            ltRole.Text = user.Role;
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            // Get form values
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validate new password
            if (newPassword.Length < 8 || newPassword.Length > 25 || !ValidationUtil.ValidatePassword(newPassword))
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = "New password must be alphanumeric and 8 to 25 characters.";
                return;
            }

            // Validate confirm password
            if (newPassword != confirmPassword)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = "New password and confirm password do not match.";
                return;
            }

            // Get user ID from session
            int userId = (int)SessionUtil.GetSession(Session, "UserId");

            // Change password
            string errorMessage;
            bool success = _userController.ChangePassword(userId, oldPassword, newPassword, out errorMessage);

            if (!success)
            {
                // Show error message
                pnlAlert.Visible = true;
                ltError.Text = errorMessage;
                return;
            }

            // Clear form
            txtOldPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";

            // Show success message
            pnlSuccess.Visible = true;
            ltSuccess.Text = "Password changed successfully.";
        }
    }
}
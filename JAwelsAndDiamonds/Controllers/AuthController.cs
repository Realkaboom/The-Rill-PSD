using System;
using System.Web;
using System.Web.UI;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Controllers
{
    /// <summary>
    /// Controller for authentication-related operations
    /// </summary>
    public class AuthController
    {
        private readonly AuthHandler _authHandler;
        private readonly Page _page;

        /// <summary>
        /// Constructor for AuthController
        /// </summary>
        /// <param name="authHandler">Authentication handler</param>
        /// <param name="page">The current page</param>
        public AuthController(AuthHandler authHandler, Page page)
        {
            _authHandler = authHandler;
            _page = page;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="confirmPassword">Confirm password</param>
        /// <param name="gender">User gender</param>
        /// <param name="dateOfBirth">User date of birth</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if registration is successful, otherwise false</returns>
        public bool Register(string email, string username, string password, string confirmPassword, string gender, DateTime dateOfBirth, out string errorMessage)
        {
            errorMessage = "";

            // Validate email
            if (string.IsNullOrEmpty(email) || !ValidationUtil.ValidateEmail(email))
            {
                errorMessage = "Invalid email format.";
                return false;
            }

            // Validate username
            if (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 25)
            {
                errorMessage = "Username must be between 3 to 25 characters.";
                return false;
            }

            // Validate password
            if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 20 || !ValidationUtil.ValidatePassword(password))
            {
                errorMessage = "Password must be alphanumeric and 8 to 20 characters.";
                return false;
            }

            // Validate confirm password
            if (password != confirmPassword)
            {
                errorMessage = "Password and confirm password do not match.";
                return false;
            }

            // Validate gender
            if (gender != "Male" && gender != "Female")
            {
                errorMessage = "Gender must be Male or Female.";
                return false;
            }

            // Validate date of birth
            if (dateOfBirth >= new DateTime(2010, 1, 1))
            {
                errorMessage = "Date of birth must be earlier than 01/01/2010.";
                return false;
            }

            // Register the user
            User newUser = _authHandler.RegisterUser(email, username, password, gender, dateOfBirth);
            if (newUser == null)
            {
                errorMessage = "Registration failed. Email might already be registered.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="rememberMe">Whether to remember the user</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if login is successful, otherwise false</returns>
        public bool Login(string email, string password, bool rememberMe, out string errorMessage)
        {
            errorMessage = "";

            // Validate email and password
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                errorMessage = "Email and password must be filled.";
                return false;
            }

            // Attempt to login
            User user = _authHandler.LoginUser(email, password);
            if (user == null)
            {
                errorMessage = "Invalid email or password.";
                return false;
            }

            // Set session variables
            SessionUtil.SetSession(_page.Session, "UserId", user.UserId);
            SessionUtil.SetSession(_page.Session, "Username", user.Username);
            SessionUtil.SetSession(_page.Session, "Role", user.Role);

            // Create cookie if remember me is checked
            if (rememberMe)
            {
                HttpCookie cookie = _authHandler.CreateUserCookie(user.UserId, user.Email, true);
                _page.Response.Cookies.Add(cookie);
            }

            return true;
        }

        /// <summary>
        /// Logs out a user
        /// </summary>
        public void Logout()
        {
            // Clear session
            SessionUtil.ClearSession(_page.Session);

            // Clear authentication cookie
            HttpCookie cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            _page.Response.Cookies.Add(cookie);

            // Redirect to home page
            _page.Response.Redirect("~/Views/Home/Index.aspx");
        }
    }
}
using System;
using System.Web.UI;
using JAwelsAndDiamonds.Handlers;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Utils;

namespace JAwelsAndDiamonds.Controllers
{
    /// <summary>
    /// Controller for user-related operations
    /// </summary>
    public class UserController
    {
        private readonly UserHandler _userHandler;
        private readonly Page _page;

        /// <summary>
        /// Constructor for UserController
        /// </summary>
        /// <param name="userHandler">User handler</param>
        /// <param name="page">The current page</param>
        public UserController(UserHandler userHandler, Page page)
        {
            _userHandler = userHandler;
            _page = page;
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>The user if found, otherwise null</returns>
        public User GetUserById(int userId)
        {
            return _userHandler.GetUserById(userId);
        }

        /// <summary>
        /// Changes a user's password
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="errorMessage">Output parameter for error message</param>
        /// <returns>True if the password was changed successfully, otherwise false</returns>
        public bool ChangePassword(int userId, string oldPassword, string newPassword, out string errorMessage)
        {
            errorMessage = "";

            // Validate old password
            if (string.IsNullOrEmpty(oldPassword))
            {
                errorMessage = "Old password must be filled.";
                return false;
            }

            // Validate new password
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 8 || newPassword.Length > 25 || !ValidationUtil.ValidatePassword(newPassword))
            {
                errorMessage = "New password must be alphanumeric and 8 to 25 characters.";
                return false;
            }

            // Change the password
            bool success = _userHandler.ChangeUserPassword(userId, oldPassword, newPassword);
            if (!success)
            {
                errorMessage = "Failed to change password. Old password might be incorrect.";
                return false;
            }

            return true;
        }
    }
}
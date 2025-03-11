using System;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets a user by email
        /// </summary>
        /// <param name="email">The email to search for</param>
        /// <returns>The user if found, otherwise null</returns>
        User GetByEmail(string email);

        /// <summary>
        /// Gets a user by username
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>The user if found, otherwise null</returns>
        User GetByUsername(string username);

        /// <summary>
        /// Validates user credentials
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>The user if validation succeeds, otherwise null</returns>
        User ValidateUser(string email, string password);

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="newPassword">New password</param>
        /// <returns>True if successful, otherwise false</returns>
        bool ChangePassword(int userId, string newPassword);
    }
}
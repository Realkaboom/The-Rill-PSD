using System;
using JAwelsAndDiamonds.Models;

namespace JAwelsAndDiamonds.Factories
{
    /// <summary>
    /// Factory for creating User objects
    /// </summary>
    public class UserFactory : IFactory<User>
    {
        /// <summary>
        /// Creates a new empty User instance
        /// </summary>
        /// <returns>A new User instance</returns>
        public User Create()
        {
            return new User();
        }

        /// <summary>
        /// Creates a new customer User
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="gender">User gender</param>
        /// <param name="dateOfBirth">User date of birth</param>
        /// <returns>A new User instance with Customer role</returns>
        public User CreateCustomer(string email, string username, string password, string gender, DateTime dateOfBirth)
        {
            return new User
            {
                Email = email,
                Username = username,
                Password = password,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                Role = "Customer"
            };
        }

        /// <summary>
        /// Creates a new admin User
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="gender">User gender</param>
        /// <param name="dateOfBirth">User date of birth</param>
        /// <returns>A new User instance with Admin role</returns>
        public User CreateAdmin(string email, string username, string password, string gender, DateTime dateOfBirth)
        {
            return new User
            {
                Email = email,
                Username = username,
                Password = password,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                Role = "Admin"
            };
        }
    }
}
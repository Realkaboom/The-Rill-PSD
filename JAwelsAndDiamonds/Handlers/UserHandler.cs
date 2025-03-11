using System;
using JAwelsAndDiamonds.Factories;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;

namespace JAwelsAndDiamonds.Handlers
{
    /// <summary>
    /// Handler for user-related business logic
    /// </summary>
    public class UserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly UserFactory _userFactory;

        /// <summary>
        /// Constructor for UserHandler
        /// </summary>
        /// <param name="userRepository">User repository</param>
        /// <param name="userFactory">User factory</param>
        public UserHandler(IUserRepository userRepository, UserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>The user if found, otherwise null</returns>
        public User GetUserById(int userId)
        {
            return _userRepository.GetById(userId);
        }

        /// <summary>
        /// Updates a user's profile
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="username">New username</param>
        /// <param name="gender">New gender</param>
        /// <param name="dateOfBirth">New date of birth</param>
        /// <returns>True if the update was successful, otherwise false</returns>
        public bool UpdateUserProfile(int userId, string username, string gender, DateTime dateOfBirth)
        {
            try
            {
                User user = _userRepository.GetById(userId);
                if (user == null)
                    return false;

                user.Username = username;
                user.Gender = gender;
                user.DateOfBirth = dateOfBirth;

                _userRepository.Update(user);
                _userRepository.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Changes a user's password
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>True if the password was changed successfully, otherwise false</returns>
        public bool ChangeUserPassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                User user = _userRepository.GetById(userId);
                if (user == null || user.Password != oldPassword)
                    return false;

                user.Password = newPassword;
                _userRepository.Update(user);
                _userRepository.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates if the user credentials are correct
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>The user if validation succeeds, otherwise null</returns>
        public User ValidateUser(string email, string password)
        {
            return _userRepository.ValidateUser(email, password);
        }

        /// <summary>
        /// Checks if an email is already registered
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if the email is already registered, otherwise false</returns>
        public bool IsEmailRegistered(string email)
        {
            return _userRepository.GetByEmail(email) != null;
        }

        /// <summary>
        /// Registers a new customer
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="gender">User gender</param>
        /// <param name="dateOfBirth">User date of birth</param>
        /// <returns>The newly created user if registration succeeds, otherwise null</returns>
        public User RegisterCustomer(string email, string username, string password, string gender, DateTime dateOfBirth)
        {
            try
            {
                if (IsEmailRegistered(email))
                    return null;

                User newUser = _userFactory.CreateCustomer(email, username, password, gender, dateOfBirth);
                _userRepository.Add(newUser);
                _userRepository.SaveChanges();

                return newUser;
            }
            catch
            {
                return null;
            }
        }
    }
}
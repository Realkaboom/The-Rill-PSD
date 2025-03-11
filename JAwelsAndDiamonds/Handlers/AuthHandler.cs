using System;
using System.Web;
using System.Web.Security;
using JAwelsAndDiamonds.Models;
using JAwelsAndDiamonds.Repositories;
using JAwelsAndDiamonds.Factories;

namespace JAwelsAndDiamonds.Handlers
{
    /// <summary>
    /// Handler for authentication-related business logic
    /// </summary>
    public class AuthHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly UserFactory _userFactory;

        /// <summary>
        /// Constructor for AuthHandler
        /// </summary>
        /// <param name="userRepository">User repository</param>
        /// <param name="userFactory">User factory</param>
        public AuthHandler(IUserRepository userRepository, UserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="gender">User gender</param>
        /// <param name="dateOfBirth">User date of birth</param>
        /// <returns>The newly registered user if successful, otherwise null</returns>
        public User RegisterUser(string email, string username, string password, string gender, DateTime dateOfBirth)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
                    return null;

                if (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 25)
                    return null;

                if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 20 || !IsAlphanumeric(password))
                    return null;

                if (gender != "Male" && gender != "Female")
                    return null;

                if (dateOfBirth >= new DateTime(2010, 1, 1))
                    return null;

                // Check if email is already registered
                if (_userRepository.GetByEmail(email) != null)
                    return null;

                // Create and register the user
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

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <returns>The logged in user if successful, otherwise null</returns>
        public User LoginUser(string email, string password)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return null;

                // Validate user credentials
                User user = _userRepository.ValidateUser(email, password);
                return user;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a user cookie for "Remember Me" functionality
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="email">User email</param>
        /// <param name="rememberMe">Whether to remember the user</param>
        /// <returns>The created cookie</returns>
        public HttpCookie CreateUserCookie(int userId, string email, bool rememberMe)
        {
            // Create a FormsAuthentication ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,                                  // ticket version
                email,                              // username
                DateTime.Now,                       // issue time
                DateTime.Now.AddDays(rememberMe ? 14 : 1), // expiration time (14 days if remember me, 1 day otherwise)
                rememberMe,                         // persistent cookie
                userId.ToString(),                  // user data
                FormsAuthentication.FormsCookiePath // cookie path
            );

            // Encrypt the ticket
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            // Create a cookie with the encrypted ticket
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            // Set cookie expiration
            if (rememberMe)
                cookie.Expires = ticket.Expiration;

            return cookie;
        }

        /// <summary>
        /// Checks if a string is a valid email
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if the email is valid, otherwise false</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a string is alphanumeric
        /// </summary>
        /// <param name="str">String to check</param>
        /// <returns>True if the string is alphanumeric, otherwise false</returns>
        private bool IsAlphanumeric(string str)
        {
            bool hasLetter = false;
            bool hasDigit = false;

            foreach (char c in str)
            {
                if (char.IsLetter(c))
                    hasLetter = true;
                else if (char.IsDigit(c))
                    hasDigit = true;
            }

            return hasLetter && hasDigit;
        }
    }
}
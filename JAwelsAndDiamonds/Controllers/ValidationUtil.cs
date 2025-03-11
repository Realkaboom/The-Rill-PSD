using System;
using System.Text.RegularExpressions;

namespace JAwelsAndDiamonds.Utils
{
    /// <summary>
    /// Utility class for validation operations
    /// </summary>
    public static class ValidationUtil
    {
        /// <summary>
        /// Validates an email address
        /// </summary>
        /// <param name="email">Email to validate</param>
        /// <returns>True if the email is valid, otherwise false</returns>
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                // Use a regular expression to validate the email
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates a password
        /// </summary>
        /// <param name="password">Password to validate</param>
        /// <returns>True if the password is valid, otherwise false</returns>
        public static bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // Check length
            if (password.Length < 8 || password.Length > 20)
                return false;

            // Check if alphanumeric (contains at least one letter and one digit)
            bool hasLetter = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsLetter(c))
                    hasLetter = true;
                else if (char.IsDigit(c))
                    hasDigit = true;

                if (hasLetter && hasDigit)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Validates a username
        /// </summary>
        /// <param name="username">Username to validate</param>
        /// <returns>True if the username is valid, otherwise false</returns>
        public static bool ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return false;

            // Check length
            return username.Length >= 3 && username.Length <= 25;
        }

        /// <summary>
        /// Validates a date
        /// </summary>
        /// <param name="date">Date to validate</param>
        /// <param name="minDate">Minimum allowed date</param>
        /// <returns>True if the date is valid, otherwise false</returns>
        public static bool ValidateDate(DateTime date, DateTime minDate)
        {
            return date >= minDate;
        }
    }
}
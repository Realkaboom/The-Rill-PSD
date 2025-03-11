using System.Web.SessionState;

namespace JAwelsAndDiamonds.Utils
{
    /// <summary>
    /// Utility class for session operations
    /// </summary>
    public static class SessionUtil
    {
        /// <summary>
        /// Sets a value in the session
        /// </summary>
        /// <param name="session">The session object</param>
        /// <param name="key">Key for the session value</param>
        /// <param name="value">Value to store</param>
        public static void SetSession(HttpSessionState session, string key, object value)
        {
            session[key] = value;
        }

        /// <summary>
        /// Gets a value from the session
        /// </summary>
        /// <param name="session">The session object</param>
        /// <param name="key">Key for the session value</param>
        /// <returns>The value from the session, or null if not found</returns>
        public static object GetSession(HttpSessionState session, string key)
        {
            return session[key];
        }

        /// <summary>
        /// Clears all session variables
        /// </summary>
        /// <param name="session">The session object</param>
        public static void ClearSession(HttpSessionState session)
        {
            session.Clear();
        }
    }
}
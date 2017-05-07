using System.Configuration;
using System.Web;
using System.Web.Security;
using ReEvolveCSharpLibrary.Containers;

namespace ReEvolveCSharpLibrary.Helpers
{
    /// <summary>
	/// Connection helper methods.
	/// </summary>
	public class ConnectionHelper
    {
        #region Public methods.

        /// <summary>
        /// Gets connection information without any information about the logged-in user.
        /// </summary>
        /// <returns>Connection information without user information.</returns>
        public static ConnectionInfo GetConnectionInfoNoUserInfo()
        {
            ConnectionInfo connectionInfo = new ConnectionInfo();

            //initializes from the webconfig keys
            if (ConfigurationManager.ConnectionStrings["Database"] != null)
            {
                connectionInfo.DatabaseConnectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            }

            return connectionInfo;
        }

        /// <summary>
        /// Gets connection information with information about the logged-in user.
        /// </summary>
        /// <returns>Connection information with user information.</returns>
        public static ConnectionInfo GetConnectionInfo()
        {
            ConnectionInfo connectionInfo = GetConnectionInfoNoUserInfo();

            #region Get the user name and the id of the currently logged-in user.

            if (HttpContext.Current.Request.IsAuthenticated)
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    try
                    {
                        // Get saved values from the authentication cookie.
                        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                        if (authTicket != null)
                        {
                            connectionInfo.UserName = authTicket.Name;

                            string contactIdString = authTicket.UserData;

                            if (!string.IsNullOrWhiteSpace(contactIdString))
                            {
                                string username = contactIdString;

                                if (username != "")
                                {
                                    connectionInfo.UserName = username;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }

            #endregion

            return connectionInfo;
        }

        #endregion
    }
}

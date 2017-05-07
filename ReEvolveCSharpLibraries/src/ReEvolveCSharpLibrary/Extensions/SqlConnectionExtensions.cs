using System.Data.SqlClient;
using ReEvolveCSharpLibrary.ErrorHandling;

namespace ReEvolveCSharpLibrary.Extensions
{
    /// <summary>
    /// Provides a set of extension methods adding retry capabilities into the standard 
    /// <see cref="System.Data.SqlClient.SqlConnection"/> implementation.
    /// </summary>
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Opens a database connection with the connection settings specified in the ConnectionString 
        /// property of the connection object. Uses the specified retry policy when opening the connection.
        /// </summary>
        /// <param name="connection">The connection object that is required as per extension method declaration.</param>
        /// <param name="retryPolicy">The retry policy defining whether to retry a request if the connection
        /// fails to be opened.</param>
        public static void OpenWithRetry(this SqlConnection connection, RetryPolicy retryPolicy)
        {
            // Check if retry policy was specified, if not, use the default retry policy.
            (retryPolicy != null ? retryPolicy : RetryPolicy.NoRetry).ExecuteAction(() =>
            {
                connection.Open();
            });
        }
    }
}

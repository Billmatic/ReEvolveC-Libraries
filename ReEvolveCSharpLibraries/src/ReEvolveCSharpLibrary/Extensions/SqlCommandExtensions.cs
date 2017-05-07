using System;
using System.Data;
using System.Data.SqlClient;
using ReEvolveCSharpLibrary.ErrorHandling;

namespace ReEvolveCSharpLibrary.Extensions
{
    /// <summary>
    /// Provides a set of extension methods adding retry capabilities into the standard 
    /// System.Data.SqlClient.SqlCommand implementation.
    /// </summary>
    public static class SqlCommandExtensions
    {
        #region Public methods.

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// Uses the specified retry policy when executing the command. Uses a separate specified retry
        /// policy when establishing a connection.
        /// </summary>
        /// <param name="command">The command object that is required as per extension method declaration.</param>
        /// <param name="cmdRetryPolicy">The command retry policy defining whether to retry a command if it 
        /// fails while executing.</param>
        /// <param name="conRetryPolicy">The connection retry policy defining whether to re-establish a 
        /// connection if it drops while executing the command.</param>
        /// <returns>The number of rows affected.</returns>
        public static int ExecuteNonQueryWithRetry(this SqlCommand command, RetryPolicy cmdRetryPolicy,
            RetryPolicy conRetryPolicy)
        {
            #region Validate arguments.

            if (command.Connection == null)
            {
                throw new InvalidOperationException("Connection property has not been initialized.");
            }

            #endregion

            // Check if retry policy was specified, if not, use the default retry policy.
            return (cmdRetryPolicy ?? RetryPolicy.NoRetry).ExecuteAction(() =>
            {
                bool hasOpenedConnection = EnsureValidConnection(command, conRetryPolicy);

                try
                {
                    return command.ExecuteNonQuery();
                }
                finally
                {
                    if (hasOpenedConnection && command.Connection != null &&
                        command.Connection.State == ConnectionState.Open)
                    {
                        command.Connection.Close();
                    }
                }
            });
        }

        /// <summary>
        /// Sends the specified command to the connection and builds a SqlDataReader object containing the
        /// results. Uses the specified retry policy when executing the command. Uses a separate specified 
        /// retry policy when establishing a connection.
        /// </summary>
        /// <param name="command">The command object that is required as per extension method declaration.</param>
        /// <param name="cmdRetryPolicy">The command retry policy defining whether to retry a command if it
        /// fails while executing.</param>
        /// <param name="conRetryPolicy">The connection retry policy defining whether to re-establish a 
        /// connection if it drops while executing the command.</param>
        /// <returns>A System.Data.SqlClient.SqlDataReader object.</returns>
        public static SqlDataReader ExecuteReaderWithRetry(this SqlCommand command,
            RetryPolicy cmdRetryPolicy, RetryPolicy conRetryPolicy)
        {
            #region Validate arguments.

            if (command.Connection == null)
            {
                throw new InvalidOperationException("Connection property has not been initialized.");
            }

            #endregion

            // Check if retry policy was specified, if not, use the default retry policy.
            return (cmdRetryPolicy ?? RetryPolicy.NoRetry).ExecuteAction(() =>
            {
                bool hasOpenedConnection = EnsureValidConnection(command, conRetryPolicy);

                try
                {
                    return command.ExecuteReader();
                }
                catch (Exception)
                {
                    if (hasOpenedConnection && command.Connection != null &&
                        command.Connection.State == ConnectionState.Open)
                    {
                        command.Connection.Close();
                    }

                    throw;
                }
            });
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by
        /// the query. Additional columns or rows are ignored. Uses the specified retry policy when 
        /// executing the command. Uses a separate specified retry policy when establishing a connection.
        /// </summary>
        /// <param name="command">The command object that is required as per extension method declaration.</param>
        /// <param name="cmdRetryPolicy">The command retry policy defining whether to retry a command if it 
        /// fails while executing.</param>
        /// <param name="conRetryPolicy">The connection retry policy defining whether to re-establish a
        /// connection if it drops while executing the command.</param>
        /// <returns> The first column of the first row in the result set, or a null reference if the result 
        /// set is empty. Returns a maximum of 2033 characters.</returns>
        public static object ExecuteScalarWithRetry(this SqlCommand command, RetryPolicy cmdRetryPolicy,
            RetryPolicy conRetryPolicy)
        {
            #region Validate arguments.

            if (command.Connection == null)
            {
                throw new InvalidOperationException("Connection property has not been initialized.");
            }

            #endregion

            // Check if retry policy was specified, if not, use the default retry policy.
            return (cmdRetryPolicy ?? RetryPolicy.NoRetry).ExecuteAction(() =>
            {
                bool hasOpenedConnection = EnsureValidConnection(command, conRetryPolicy);

                try
                {
                    return command.ExecuteScalar();
                }
                finally
                {
                    if (hasOpenedConnection && command.Connection != null &&
                        command.Connection.State == ConnectionState.Open)
                    {
                        command.Connection.Close();
                    }
                }
            });
        }

        #endregion

        #region Private methods.

        /// <summary>
        /// Checks if the connection in a command is valid.
        /// </summary>
        /// <param name="command">SQL command.</param>
        /// <param name="retryPolicy">Retry policy.</param>
        /// <returns>true if the connection is valid and open; otherwise false.</returns>
        private static bool EnsureValidConnection(SqlCommand command, RetryPolicy retryPolicy)
        {
            if (command != null)
            {
                #region Validate arguments.

                if (command.Connection == null)
                {
                    throw new InvalidOperationException("Connection property has not been initialized.");
                }

                #endregion

                // Verify whether or not the connection is valid and is open. 
                // This code may be retried therefore it is important to ensure that 
                // a connection is re-established should it have previously failed.
                if (command.Connection.State != ConnectionState.Open)
                {
                    // Attempt to open the connection using the retry policy that 
                    // matches the policy for SQL commands.
                    command.Connection.OpenWithRetry(retryPolicy);

                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}

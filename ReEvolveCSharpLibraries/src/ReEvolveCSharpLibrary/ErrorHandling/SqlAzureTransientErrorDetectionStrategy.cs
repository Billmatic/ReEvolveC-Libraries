using System;
using System.Data;
using System.Data.SqlClient;

namespace ReEvolveCSharpLibrary.ErrorHandling
{
    /// <summary>
    /// Provides the transient error detection logic for transient faults that are specific to SQL Azure.
    /// </summary>
    public sealed class SqlAzureTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region ProcessNetLibErrorCode enumeration

        /// <summary>
        /// Error codes reported by the DBNETLIB module.
        /// </summary>
        private enum ProcessNetLibErrorCode
        {
            /// <summary>
            /// Zero bytes.
            /// </summary>
            ZeroBytes = -3,

            /// <summary>
            /// Timeout expired. The timeout period elapsed prior to completion of the operation or the 
            /// server is not responding.
            /// </summary>
            Timeout = -2,

            /// <summary>
            /// Unknown error.
            /// </summary>
            Unknown = -1,

            /// <summary>
            /// Insufficient memory.
            /// </summary>
            InsufficientMemory = 1,

            /// <summary>
            /// Access denied.
            /// </summary>
            AccessDenied = 2,

            /// <summary>
            /// The connection is busy.
            /// </summary>
            ConnectionBusy = 3,

            /// <summary>
            /// The connection is broken.
            /// </summary>
            ConnectionBroken = 4,

            /// <summary>
            /// The connection limit has been reached.
            /// </summary>
            ConnectionLimit = 5,

            /// <summary>
            /// The server could not be found.
            /// </summary>
            ServerNotFound = 6,

            /// <summary>
            /// No network was found.
            /// </summary>
            NetworkNotFound = 7,

            /// <summary>
            /// There are insufficent resources.
            /// </summary>
            InsufficientResources = 8,

            /// <summary>
            /// The network is busy.
            /// </summary>
            NetworkBusy = 9,

            /// <summary>
            /// Access to the network was denied.
            /// </summary>
            NetworkAccessDenied = 10,

            /// <summary>
            /// General error.
            /// </summary>
            GeneralError = 11,

            /// <summary>
            /// Incorrect mode.
            /// </summary>
            IncorrectMode = 12,

            /// <summary>
            /// The name was not found.
            /// </summary>
            NameNotFound = 13,

            /// <summary>
            /// The connection is invalid.
            /// </summary>
            InvalidConnection = 14,

            /// <summary>
            /// Read/write error.
            /// </summary>
            ReadWriteError = 15,

            /// <summary>
            /// Too many handles.
            /// </summary>
            TooManyHandles = 16,

            /// <summary>
            /// Server error.
            /// </summary>
            ServerError = 17,

            /// <summary>
            /// SSL error.
            /// </summary>
            SSLError = 18,

            /// <summary>
            /// Encryption error.
            /// </summary>
            EncryptionError = 19,

            /// <summary>
            /// Encryption is not supported.
            /// </summary>
            EncryptionNotSupported = 20
        }

        #endregion

        #region ITransientErrorDetectionStrategy implementation

        /// <summary>
        /// Determines whether the specified exception represents a transient failure that can be compensated
        /// by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>True if the specified exception is considered as transient; otherwise false.</returns>
        public bool IsTransient(Exception ex)
        {
            if (ex != null)
            {
                SqlException sqlException;

                if ((sqlException = ex as SqlException) != null)
                {
                    // Enumerate through all errors found in the exception.
                    foreach (SqlError err in sqlException.Errors)
                    {
                        switch (err.Number)
                        {
                            case ThrottlingCondition.ThrottlingErrorNumber:
                                // SQL Error Code: 40501
                                // The service is currently busy. Retry the request after 10 seconds. Code: 
                                // (reason code to be decoded). Decode the reason code from the error 
                                // message to determine the grounds for throttling.
                                var condition = ThrottlingCondition.FromError(err);

                                // Attach the decoded values as additional attributes to the original SQL 
                                // exception.
                                sqlException.Data[condition.ThrottlingMode.GetType().Name] =
                                    condition.ThrottlingMode.ToString();
                                sqlException.Data[condition.GetType().Name] = condition;

                                return true;

                            case 40197:
                            // SQL Error Code: 40197
                            // The service has encountered an error processing your request. Please try 
                            // again.
                            case 10053:
                            // SQL Error Code: 10053
                            // A transport-level error has occurred when receiving results from the 
                            // server. An established connection was aborted by the software in your 
                            // host machine.
                            case 10054:
                            // SQL Error Code: 10054
                            // A transport-level error has occurred when sending the request to the 
                            // server. (provider: TCP Provider, error: 0 - An existing connection was
                            // forcibly closed by the remote host.)
                            case 10060:
                            // SQL Error Code: 10060
                            // A network-related or instance-specific error occurred while establishing 
                            // a connection to SQL Server. The server was not found or was not 
                            // accessible. Verify that the instance name is correct and that SQL Server 
                            // is configured to allow remote connections. (provider: TCP Provider, 
                            // error: 0 - A connection attempt failed because the connected party did not
                            // properly respond after a period of time, or established connection failed 
                            // because connected host has failed to respond.)"}
                            case 40613:
                            // SQL Error Code: 40613
                            // Database XXXX on server YYYY is not currently available. Please retry the
                            // connection later. If the problem persists, contact customer support, and
                            // provide them the session tracing ID of ZZZZZ.
                            case 40143:
                            // SQL Error Code: 40143
                            // The service has encountered an error processing your request. Please try
                            // again.
                            case 233:
                            // SQL Error Code: 233
                            // The client was unable to establish a connection because of an error during
                            // connection initialization process before login. Possible causes include 
                            // the following: the client tried to connect to an unsupported version of 
                            // SQL Server; the server was too busy to accept new connections; or there 
                            // was a resource limitation (insufficient memory or maximum allowed 
                            // connections) on the server. (provider: TCP Provider, error: 0 - An 
                            // existing connection was forcibly closed by the remote host.)
                            case 64:
                            // SQL Error Code: 64
                            // A connection was successfully established with the server, but then an 
                            // error occurred during the login process. (provider: TCP Provider, error: 
                            // 0 - The specified network name is no longer available.) 
                            case (int)ProcessNetLibErrorCode.EncryptionNotSupported:
                                // DBNETLIB Error Code: 20
                                // The instance of SQL Server you attempted to connect to does not support
                                // encryption.
                                return true;
                        }
                    }
                }
                else if (ex is TimeoutException)
                {
                    return true;
                }
                else
                {
                    EntityException entityException;

                    if ((entityException = ex as EntityException) != null)
                    {
                        return this.IsTransient(entityException.InnerException);
                    }
                }
            }

            return false;
        }

        #endregion
    }
}

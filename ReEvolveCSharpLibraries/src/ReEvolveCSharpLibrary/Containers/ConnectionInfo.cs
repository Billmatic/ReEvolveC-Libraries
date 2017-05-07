
namespace ReEvolveCSharpLibrary.Containers
{
    /// <summary>
	/// data access connection information.
	/// </summary>
	public class ConnectionInfo
    {
        #region Properties.

        /// <summary>
        /// Gets or sets the connection string for the database.
        /// </summary>
        /// <value>Database connection string.</value>
        public string DatabaseConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the connection string for azure storage.
        /// </summary>
        /// <value>Storage connection string.</value>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the current user.
        /// </summary>
        /// <value>Current user name.</value>
        public string UserName { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ReEvolveCSharpLibrary.ErrorHandling;
using ReEvolveCSharpLibrary.Extensions;


namespace ReEvolveDataLayerProject.Common
{
    /// <summary>
	/// Data manager.
	/// </summary>
	public class DataManager
    {
        #region Private constants.

        /// <summary>Default database command timeout in seconds.</summary>
        private const int DefaultCommandTimeout = 300;

        #endregion

        #region Private variables.

        /// <summary>Database command timeout in seconds.</summary>
        private int _commandTimeout;

        /// <summary>Database connection string.</summary>
        private string _connectionString;

        #endregion

        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        public DataManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        /// <param name="databaseConnectionString">Database connection string.</param>
        public DataManager(string databaseConnectionString)
            : this(databaseConnectionString, DefaultCommandTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        /// <param name="databaseConnectionString">Database connection string.</param>
        /// <param name="commandTimeout">Database command timeout in seconds.</param>
        public DataManager(string databaseConnectionString, int commandTimeout)
        {
            #region Validate arguments.

            if (string.IsNullOrWhiteSpace(databaseConnectionString))
            {
                throw new ArgumentNullException("databaseConnectionString");
            }

            #endregion

            Initialize(databaseConnectionString, commandTimeout);
        }

        #endregion

        #region Public methods.

        #region Database object methods.

        /// <summary>
        /// Creates a database command.
        /// </summary>
        /// <returns>Database command.</returns>
        public SqlCommand GetCommand()
        {
            SqlCommand command = new SqlCommand
            {
                CommandTimeout = _commandTimeout
            };

            return command;
        }

        /// <summary>
        /// Creates a database connection.
        /// </summary>
        /// <returns>Database connection.</returns>
        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = _connectionString
            };

            RetryPolicy retryPolicy = GetRetryPolicy();

            connection.OpenWithRetry(retryPolicy);

            return connection;
        }

        #region Parameter methods.

        /// <summary>
        /// Returns a database parameter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="dataType">Data type.</param>
        /// <returns>Database parameter.</returns>
        public SqlParameter GetParameter(string name, DbType dataType)
        {
            return GetParameter(name, dataType, null, null, null, ParameterDirection.Input, null);
        }

        /// <summary>
        /// Returns a database parameter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="value">Value.</param>
        /// <returns>Database parameter.</returns>
        public SqlParameter GetParameter(string name, DbType dataType, object value)
        {
            return GetParameter(name, dataType, null, null, null, ParameterDirection.Input, value);
        }

        /// <summary>
        /// Returns a database parameter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="direction">Direction.</param>
        /// <returns>Database parameter.</returns>
        public SqlParameter GetParameter(string name, DbType dataType, ParameterDirection direction)
        {
            return GetParameter(name, dataType, null, null, null, direction, null);
        }

        /// <summary>
        /// Returns a database parameter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="value">Value.</param>
        /// <returns>Database parameter.</returns>
        public SqlParameter GetParameter(string name, DbType dataType, ParameterDirection direction,
            object value)
        {
            return GetParameter(name, dataType, null, null, null, direction, value);
        }

        /// <summary>
        /// Returns a database parameter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="size">Size.</param>
        /// <param name="precision">Precision.</param>
        /// <param name="scale">Scale.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="value">Value.</param>
        /// <returns>Database parameter.</returns>
        public SqlParameter GetParameter(string name, DbType dataType,
            int? size, int? precision, int? scale, ParameterDirection direction,
            object value)
        {
            SqlParameter parameter = new SqlParameter();

            parameter.DbType = dataType;
            parameter.Direction = direction;

            if (!string.IsNullOrWhiteSpace(name))
            {
                parameter.ParameterName = name;
            }

            if (precision != null)
            {
                parameter.Precision = Convert.ToByte(precision);
            }

            if (scale != null)
            {
                parameter.Scale = Convert.ToByte(scale);
            }

            if (size != null)
            {
                parameter.Size = Convert.ToInt32(size);
            }

            if (value != null)
            {
                parameter.Value = value;
            }

            return parameter;
        }

        #endregion

        #endregion

        #region Execute methods.

        #region DataSet methods.

        /// <summary>
        /// Loads a DataSet with the results of the specified command.
        /// </summary>
        /// <param name="command">Database command.</param>
        /// <param name="tables">DataTable names.</param>
        /// <returns>Result set.</returns>
        public DataSet ExecuteDataSet(SqlCommand command, string[] tables)
        {
            return ExecuteDataSet(command, tables, new List<SqlParameter>());
        }

        /// <summary>
        /// Loads a DataSet with the results of the specified command.
        /// </summary>
        /// <param name="command">Database command.</param>
        /// <param name="tables">DataTable names.</param>
        /// <param name="parameters">Database command parameters.</param>
        /// <returns>Result set.</returns>
        public DataSet ExecuteDataSet(SqlCommand command, string[] tables, List<SqlParameter> parameters)
        {
            DataSet dataSet = new DataSet();

            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            using (SqlConnection connection = GetConnection())
            {
                RetryPolicy retryPolicy = GetRetryPolicy();

                command.Connection = connection;

                using (SqlDataReader reader = command.ExecuteReaderWithRetry(retryPolicy, retryPolicy))
                {
                    dataSet.Load(reader, LoadOption.OverwriteChanges, tables);
                }
            }

            // Clear parameters to avoid problems on subsequent calls.
            command.Parameters.Clear();

            return dataSet;
        }

        /// <summary>
        /// Loads a DataSet with the results of the specified stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <param name="tables">DataTable names.</param>
        /// <returns>Result set.</returns>
        public DataSet ExecuteDataSet(string storedProcedureName, string[] tables)
        {
            return ExecuteDataSet(storedProcedureName, tables, new List<SqlParameter>());
        }

        /// <summary>
        /// Loads a DataSet with the results of the specified stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <param name="tables">DataTable names.</param>
        /// <param name="parameters">Stored procedure parameters.</param>
        /// <returns>Result set.</returns>
        public DataSet ExecuteDataSet(string storedProcedureName, string[] tables,
            List<SqlParameter> parameters)
        {
            ValidateParameters(parameters);

            SqlCommand command = GetCommand();

            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;

            return ExecuteDataSet(command, tables, parameters);
        }

        #endregion

        #region DataTable methods.

        /// <summary>
        /// Loads a DataTable with the results of the specified command.
        /// </summary>
        /// <param name="command">Database command.</param>
        /// <returns>Result set.</returns>
        public DataTable ExecuteDataTable(SqlCommand command)
        {
            return ExecuteDataTable(command, new List<SqlParameter>());
        }

        /// <summary>
        /// Loads a DataTable with the results of the specified command.
        /// </summary>
        /// <param name="command">Database command.</param>
        /// <param name="parameters">Database command parameters.</param>
        /// <returns>Result set.</returns>
        public DataTable ExecuteDataTable(SqlCommand command, List<SqlParameter> parameters)
        {
            DataTable dataTable = new DataTable();

            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            using (SqlConnection connection = GetConnection())
            {
                RetryPolicy retryPolicy = GetRetryPolicy();

                command.Connection = connection;

                using (SqlDataReader reader = command.ExecuteReaderWithRetry(retryPolicy, retryPolicy))
                {
                    dataTable.Load(reader);
                }
            }

            // Clear parameters to avoid problems on subsequent calls.
            command.Parameters.Clear();

            return dataTable;
        }

        /// <summary>
        /// Loads a DataTable with the results of the specified stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <returns>Result set.</returns>
        public DataTable ExecuteDataTable(string storedProcedureName)
        {
            return ExecuteDataTable(storedProcedureName, new List<SqlParameter>());
        }

        /// <summary>
        /// Loads a DataTable with the results of the specified stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <param name="parameters">Stored procedure parameters.</param>
        /// <returns>Result set.</returns>
        public DataTable ExecuteDataTable(string storedProcedureName, List<SqlParameter> parameters)
        {
            ValidateParameters(parameters);

            SqlCommand command = GetCommand();

            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;

            return ExecuteDataTable(command, parameters);
        }

        #endregion

        #region Non-query methods.

        /// <summary>
        /// Executes the specified stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <returns>Stored procedure return value.</returns>
        public int ExecuteNonQuery(string storedProcedureName)
        {
            return ExecuteNonQuery(storedProcedureName, new List<SqlParameter>());
        }

        /// <summary>
        /// Executes the specified stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <param name="parameters">Stored procedure parameters.</param>
        /// <returns>Stored procedure return value.</returns>
        public int ExecuteNonQuery(string storedProcedureName, List<SqlParameter> parameters)
        {
            ValidateParameters(parameters);

            int returnValue = 0;

            using (SqlCommand command = GetCommand())
            {
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                SqlParameter returnValueParam = null;

                if (command.Parameters.Contains("ReturnValue"))
                {
                    returnValueParam = (SqlParameter)command.Parameters["ReturnValue"];
                }
                else
                {
                    returnValueParam = GetParameter("ReturnValue", DbType.Int32,
                        ParameterDirection.ReturnValue);

                    command.Parameters.Add(returnValueParam);
                }

                using (SqlConnection connection = GetConnection())
                {
                    RetryPolicy retryPolicy = GetRetryPolicy();

                    command.Connection = connection;
                    command.ExecuteNonQueryWithRetry(retryPolicy, retryPolicy);

                    int.TryParse(returnValueParam.Value.ToString(), out returnValue);
                }

                // Clear parameters to avoid problems on subsequent calls.
                command.Parameters.Clear();
            }

            return returnValue;
        }

        #endregion

        #region Scalar methods.

        /// <summary>
        /// Executes the specified stored procedure and returns the first  column of the first row in the 
        /// result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <returns>First column of the first row.</returns>
        public object ExecuteScalar(string storedProcedureName)
        {
            return ExecuteScalar(storedProcedureName, new List<SqlParameter>());
        }

        /// <summary>
        /// Executes the specified stored procedure and returns the first column of the first row in the
        /// result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="storedProcedureName">Stored procedure name.</param>
        /// <param name="parameters">Stored procedure parameters.</param>
        /// <returns>First column of the first row.</returns>
        public object ExecuteScalar(string storedProcedureName, List<SqlParameter> parameters)
        {
            ValidateParameters(parameters);

            object result = null;

            using (SqlCommand command = GetCommand())
            {
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;

                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                using (SqlConnection connection = GetConnection())
                {
                    RetryPolicy retryPolicy = GetRetryPolicy();

                    command.Connection = connection;
                    result = command.ExecuteScalarWithRetry(retryPolicy, retryPolicy);
                }

                // Clear parameters to avoid problems on subsequent calls.
                command.Parameters.Clear();
            }

            return result;
        }

        #endregion

        #endregion

        #endregion

        #region Private methods.

        /// <summary>
        /// Gets a retry policy for transient error handling.
        /// </summary>
        /// <returns>Retry policy.</returns>
        private RetryPolicy GetRetryPolicy()
        {
            // Get a SQL Azure error detection strategy.
            ITransientErrorDetectionStrategy errorDetectionStrategy =
                new SqlAzureTransientErrorDetectionStrategy();

            // Get an incremental retry strategy, retrying a maximum of 3 times, waiting 1 second for the 
            // first retry, and increasing the interval by 1 additional second on each retry.
            RetryStrategy retryStrategy = new Incremental(3, TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1));

            RetryPolicy retryPolicy = new RetryPolicy(errorDetectionStrategy, retryStrategy);

            return retryPolicy;
        }

        /// <summary>
        /// Initializes the class.
        /// </summary>
        /// <param name="databaseConnectionString">Database connection string.</param>
        /// <param name="commandTimeout">Database command timeout in seconds.</param>
        private void Initialize(string databaseConnectionString, int commandTimeout)
        {
            // Set the connection string.
            _connectionString = databaseConnectionString;

            #region Set the command timeout.

            if (commandTimeout <= 0)
            {
                _commandTimeout = DefaultCommandTimeout;
            }
            else
            {
                _commandTimeout = commandTimeout;
            }

            #endregion
        }

        /// <summary>
        /// Checks to see if parameters are valid.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        private void ValidateParameters(List<SqlParameter> parameters)
        {
            foreach (SqlParameter parameter in parameters)
            {
                #region Check that scale and precision are set for decimal parameters.

                if (parameter.DbType == DbType.Decimal)
                {
                    if (parameter.Precision == 0 &&
                        parameter.Scale == 0)
                    {
                        throw new Exception(string.Format(
                            "Precision and/or scale not set on {0}",
                            parameter.ParameterName));
                    }
                }

                #endregion
            }
        }

        #endregion
    }
}

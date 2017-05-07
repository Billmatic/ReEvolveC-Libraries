using System;
using System.Data;
using System.Data.SqlClient;
using ReEvolveCSharpLibrary.Containers;
using ReEvolveCSharpLibrary.Converters;

namespace ReEvolveDataLayerProject.Common
{
    /// <summary>
	/// Data access base class.
	/// </summary>
	public class DataBase
    {
        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBase"/> class.
        /// </summary>
        /// <param name="connectionInfo">Connection information.</param>
        public DataBase(ConnectionInfo connectionInfo)
        {
            if (connectionInfo == null)
            {
                throw new ArgumentNullException("connectionInfo");
            }

            if (string.IsNullOrWhiteSpace(connectionInfo.DatabaseConnectionString))
            {
                throw new ArgumentNullException("connectionInfo.DatabaseConnectionString");
            }

            this.ConnectionInfo = connectionInfo;
            this.DataManager = new DataManager(connectionInfo.DatabaseConnectionString);
        }

        #endregion

        #region Properties.

        /// <summary>
        /// Gets or sets connection information.
        /// </summary>
        /// <value>Connection information.</value>
        public ConnectionInfo ConnectionInfo { get; set; }

        /// <summary>
        /// Gets or sets the data manager.
        /// </summary>
        /// <value>Data manager.</value>
        public DataManager DataManager { get; set; }

        #endregion

        #region Protected methods.

        /// <summary>
        /// Gets a column value from a DataRow.
        /// </summary>
        /// <typeparam name="T">Data type of the column.</typeparam>
        /// <param name="row">DataRow.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>Column value.</returns>
        protected virtual T GetDataRowValue<T>(DataRow row, string columnName)
        {
            T value = default(T);

            if (row.Table.Columns.Contains(columnName))
            {
                value = DataConverter.ConvertFromDb<T>(row[columnName]);
            }

            return value;
        }

        /// <summary>
        /// Gets a parameter for the IsActive property of an entity.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        /// <returns>SqlParameter for the IsActive property.</returns>
        protected virtual SqlParameter GetIsActiveParameter(EntityBase entity)
        {
            SqlParameter parameter = this.DataManager.GetParameter("isActive", DbType.Boolean, DataConverter.ConvertToDb(entity.IsActive));

            return parameter;
        }

        /// <summary>
        /// Gets a parameter for the modified on property of an entity.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        /// <returns>SqlParameter for the modified on property.</returns>
        protected virtual SqlParameter GetModifiedOnParameter(EntityBase entity)
        {
            SqlParameter parameter = this.DataManager.GetParameter("modifiedOn", DbType.DateTime2, ParameterDirection.InputOutput,
                DataConverter.ConvertToDb(entity.ModifiedOn));

            return parameter;
        }

        /// <summary>
        /// Gets a parameter for the primary key property of an entity.
        /// </summary>
        /// <param name="name">Name of the stored procedure parameter.</param>
        /// <param name="dataType">Data type of the stored procedure parameter.</param>
        /// <param name="value">Value of the primary key property.</param>
        /// <returns>SqlParameter for the primary key property.</returns>
        protected virtual SqlParameter GetPrimaryKeyParameter(string name, DbType dataType, object value)
        {
            SqlParameter parameter = this.DataManager.GetParameter(name, dataType, ParameterDirection.InputOutput, DataConverter.ConvertToDb(value));

            return parameter;
        }

        /// <summary>
        /// Gets a parameter for the name of the user performing an update.
        /// </summary>
        /// <returns>SqlParameter for the user name.</returns>
        protected virtual SqlParameter GetUserParameter()
        {
            if (string.IsNullOrWhiteSpace(this.ConnectionInfo.UserName))
            {
                throw new ArgumentNullException("UserName");
            }

            SqlParameter parameter = this.DataManager.GetParameter("user", DbType.String, DataConverter.ConvertToDb(this.ConnectionInfo.UserName));

            return parameter;
        }

        /// <summary>
        /// Populates base entity fields from a DataRow.
        /// </summary>
        /// <param name="entity">Entity to populate.</param>
        /// <param name="row">DataRow containing entity data.</param>
        protected virtual void PopulateEntityBaseProperties(EntityBase entity, DataRow row)
        {
            entity.CreatedBy = GetDataRowValue<string>(row, "CreatedBy");
            entity.CreatedOn = GetDataRowValue<DateTime?>(row, "CreatedOn");
            entity.IsActive = GetDataRowValue<bool?>(row, "IsActive");
            entity.ModifiedBy = GetDataRowValue<string>(row, "ModifiedBy");
            entity.ModifiedOn = GetDataRowValue<DateTime?>(row, "ModifiedOn");
        }

        /// <summary>
        /// Populates the results of a paging operation.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="pagedResults">PagedResults object.</param>
        /// <param name="row">The first DataRow in the result set.</param>
        /// <param name="pageSize">The number of records per page.</param>
        protected virtual void PopulatePagedResults<T>(PagedResults<T> pagedResults, DataRow row, int pageSize)
        {
            int rowNumber = GetDataRowValue<int>(row, "RowNumber");
            int totalCount = GetDataRowValue<int>(row, "TotalCount");

            decimal pageSizeDecimal = (decimal)pageSize;

            if (pageSizeDecimal != 0m)
            {
                pagedResults.CurrentPage = (int)decimal.Ceiling((decimal)rowNumber / pageSizeDecimal);
                pagedResults.TotalCount = totalCount;
                pagedResults.TotalPages = (int)decimal.Ceiling((decimal)totalCount / pageSizeDecimal);
            }
            else
            {
                pagedResults.CurrentPage = 0;
                pagedResults.TotalCount = 0;
                pagedResults.TotalPages = 0;
            }
        }

        #endregion
    }
}

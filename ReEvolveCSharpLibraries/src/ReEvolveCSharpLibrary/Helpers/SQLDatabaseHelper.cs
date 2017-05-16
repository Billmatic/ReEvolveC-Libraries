using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ReEvolveCSharpLibrary.Helpers
{
    public class SQLDatabaseHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SqlConnection ConnectToSQLDatabase(string serverName, string databaseName, string userName, string userPassword, string domain)
        {
            SqlConnection sqlConnection = new SqlConnection();
            if (domain != null)
            {
                sqlConnection = new SqlConnection("user id=" + domain + "\\" + userName + ";" +
                                       "password=" + userPassword + ";server=" + serverName + ";" +
                                       "Trusted_Connection=yes;" +
                                       "database=" + databaseName + "; " +
                                       "connection timeout=30");
            }
            else
            {
                sqlConnection = new SqlConnection(@"Server=tcp:"+serverName+";Database="+databaseName+";User ID="+userName+";Password="+userPassword+";MultipleActiveResultSets=True;Encrypt=true;TrustServerCertificate=false;Trusted_Connection=false;");
            }
            

            return sqlConnection;
        }

    }

    
}

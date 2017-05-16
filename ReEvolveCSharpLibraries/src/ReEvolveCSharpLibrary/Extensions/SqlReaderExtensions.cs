using System;
using System.Data.SqlClient;

namespace ReEvolveCSharpLibrary.Extensions
{
    public static class SqlReaderExtensions
    {
        private static string GetStringFromDB(this SqlDataReader reader, string columnName)
        {
            string data = "";
            try
            {
                data = reader[columnName].ToString().Trim();
            }
            catch
            {

            }

            return data;
        }

        private static bool? GetBoolFromDB(this SqlDataReader reader, string columnName)
        {
            bool? data = null;
            try
            {
                data = (bool?)reader[columnName];
            }
            catch
            {

            }

            return data;
        }

        private static DateTime GetDateTimeFromDB(this SqlDataReader reader, string columnName)
        {
            DateTime data = new DateTime();
            try
            {
                data = Convert.ToDateTime(reader[columnName].ToString());
            }
            catch
            {

            }

            return data;
        }
    }
}

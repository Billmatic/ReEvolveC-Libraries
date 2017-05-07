using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace ReEvolveCSharpLibrary.Converters
{
    /// <summary>
	/// Data conversion methods.
	/// </summary>
	public class DataConverter
    {
        #region Database conversion methods.

        /// <summary>
        /// Converts a value from a database data type to a .NET data type.
        /// </summary>
        /// <typeparam name="T">.NET data type to convert to.</typeparam>
        /// <param name="value">Database value to convert.</param>
        /// <returns>Converted value.</returns>
        public static T ConvertFromDb<T>(object value)
        {
            // Get the type to which the value will be converted.
            Type typeToConvertTo = typeof(T);

            #region Determine if the type to convert to is nullable.

            bool isNullableType = false;
            Type nullableType = null;

            // Determine if the type to convert is generic.
            if (typeToConvertTo.IsGenericType)
            {
                Type genericType = typeToConvertTo.GetGenericTypeDefinition();

                // Determine if the type to convert to is nullable.
                if (genericType == typeof(Nullable<>))
                {
                    isNullableType = true;
                    nullableType = typeToConvertTo.GetGenericArguments()[0];
                    typeToConvertTo = nullableType;
                }
            }

            #endregion

            // Check if the database value is null.
            if (value == DBNull.Value)
            {
                return default(T);
            }

            // Convert the value.
            if (isNullableType)
            {
                return (T)Convert.ChangeType(value, nullableType);
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        /// <summary>
        /// Converts a value from a .NET data type to a database data type.
        /// Values are set to database nulls only if the .NET data type is
        /// nullable.
        /// </summary>
        /// <param name="value">.NET value to convert.</param>
        /// <returns>Converted value.</returns>
        public static object ConvertToDb(object value)
        {
            // Check if the value to convert is null.
            if (value == null)
            {
                return DBNull.Value;
            }

            return value;
        }

        #endregion

        #region Time conversion methods.

        /// <summary>
        /// Converts a date/time from Eastern to UTC.
        /// </summary>
        /// <param name="easternTime">Eastern date/time.</param>
        /// <returns>UTC date/time.</returns>
        public static DateTime ConvertFromEasternToUTC(DateTime easternTime)
        {
            DateTime utcTime = easternTime;

            try
            {
                TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                utcTime = TimeZoneInfo.ConvertTimeToUtc(easternTime, est);
            }
            catch (TimeZoneNotFoundException)
            {
                Trace.TraceError("Could not find the Eastern Standard Time time zone.");
                return easternTime;
            }
            catch (InvalidTimeZoneException)
            {
                Trace.TraceError("Eastern Standard Time time zone is invalid.");
                return easternTime;
            }

            return utcTime;
        }

        /// <summary>
        /// Converts a date/time from UTC to Eastern.
        /// </summary>
        /// <param name="utcTime">UTC date/time.</param>
        /// <returns>Eastern date/time.</returns>
        public static DateTime ConvertFromUTCToEastern(DateTime utcTime)
        {
            DateTime easternTime = utcTime;

            try
            {
                TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                easternTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, est);
            }
            catch (TimeZoneNotFoundException)
            {
                Trace.TraceError("Could not find the Eastern Standard Time time zone.");
                return utcTime;
            }
            catch (InvalidTimeZoneException)
            {
                Trace.TraceError("Eastern Standard Time time zone is invalid.");
                return utcTime;
            }

            return easternTime;
        }

        #endregion

        #region Web conversion methods.

        /// <summary>
        /// Converts a string to a .NET data type.
        /// </summary>
        /// <typeparam name="T">.NET data type to convert to.</typeparam>
        /// <param name="value">String value to convert.</param>
        /// <returns>Converted value.</returns>
        public static T ConvertFromWeb<T>(string value)
        {
            // Convert empty strings to nulls or default values.
            if (value == "")
            {
                return default(T);
            }

            // Get the type to which the value will be converted.
            Type typeToConvertTo = typeof(T);

            #region Determine if the type to convert to is nullable.

            bool isNullableType = false;
            Type nullableType = null;

            // Determine if the type to convert is generic.
            if (typeToConvertTo.IsGenericType)
            {
                Type genericType = typeToConvertTo.GetGenericTypeDefinition();

                // Determine if the type to convert to is nullable.
                if (genericType == typeof(Nullable<>))
                {
                    isNullableType = true;
                    nullableType = typeToConvertTo.GetGenericArguments()[0];
                    typeToConvertTo = nullableType;
                }
            }

            #endregion

            // Convert the value.
            if (isNullableType)
            {
                if (typeof(T) == typeof(Guid?))
                {
                    if (value == null)
                    {
                        return default(T);
                    }
                    else if (value == Guid.Empty.ToString())
                    {
                        return default(T);
                    }
                    else
                    {
                        return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
                    }
                }

                return (T)Convert.ChangeType(value, nullableType);
            }
            else
            {
                if (typeof(T) == typeof(Guid))
                {
                    Guid guid = Guid.Empty;

                    Guid.TryParse(value, out guid);

                    return (T)Convert.ChangeType(guid, typeof(T));
                }
                else
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
        }

        /// <summary>
        /// Converts a string to a .NET DateTime?.
        /// </summary>
        /// <param name="value">String value to convert.</param>
        /// <param name="dateFormat">Date format to use for conversion.</param>
        /// <returns>Converted value.</returns>
        public static DateTime? ConvertFromWeb(string value, string dateFormat)
        {
            // Convert empty strings to nulls or default values.
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            DateTime? date = null;

            try
            {
                date = DateTime.ParseExact(value, dateFormat, CultureInfo.InvariantCulture);
            }
            catch
            {
            }

            return date;
        }

        /// <summary>
        /// Converts a value from a .NET data type to a string.
        /// </summary>
        /// <param name="value">.NET value to convert.</param>
        /// <returns>Converted value.</returns>
        public static string ConvertToWeb(object value)
        {
            // Convert nulls to empty strings.
            if (value == null)
            {
                return "";
            }

            return value.ToString();
        }

        #endregion
    }
}

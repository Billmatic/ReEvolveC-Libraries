using System;
using System.Web.UI;

namespace ReEvolveCSharpLibrary.Extensions
{
    public static class ViewStateExtensions
    {
        /// <summary>
        /// Adds an item to the ViewState.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <param name="value">Value of the ViewState item.</param>
        public static void Add(this StateBag viewState, object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            viewState.Add(key.ToString(), value);
        }


        /// <summary>
        /// Gets a ViewState value as a boolean.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as a boolean.</returns>
        public static bool GetBool(this StateBag viewState, object key)
        {
            bool value = false;
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                bool.TryParse(viewState[key.ToString()].ToString(), out value);
            }
            return value;
        }


        /// <summary>
        /// Gets a ViewState value as a decimal.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as a decimal.</returns>
        public static decimal GetDecimal(this StateBag viewState, object key)
        {
            decimal value = 0m;

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                decimal.TryParse(viewState[key.ToString()].ToString(), out value);
            }
            return value;
        }


        /// <summary>
        /// Gets a ViewState value as a guid.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as a guid.</returns>
        public static Guid GetGuid(this StateBag viewState, object key)
        {
            Guid value = Guid.Empty;

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                Guid.TryParse(viewState[key.ToString()].ToString(), out value);
            }
            return value;
        }


        /// <summary>
        /// Gets a ViewState value as a guid.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as a guid.</returns>
        public static Guid? GetGuidNullable(this StateBag viewState, object key)
        {

            Guid? value = null;

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                Guid convertedValue = Guid.Empty;

                if (Guid.TryParse(viewState[key.ToString()].ToString(), out convertedValue))
                {
                    value = convertedValue;
                }
            }
            return value;
        }


        /// <summary>
        /// Gets a ViewState value as an integer.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as an integer.</returns>
        public static int GetInt(this StateBag viewState, object key)
        {
            int value = 0;

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                int.TryParse(viewState[key.ToString()].ToString(), out value);
            }

            return value;
        }


        /// <summary>
        /// Gets a ViewState value as a nullable integer.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as a nullable integer.</returns>
        public static int? GetIntNullable(this StateBag viewState, object key)
        {
            int? value = null;

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                int convertedValue = 0;

                if (int.TryParse(viewState[key.ToString()].ToString(), out convertedValue))
                {
                    value = convertedValue;
                }
            }
            return value;
        }


        /// <summary>
        /// Gets a ViewState value as an object.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as an object.</returns>
        public static object GetObject(this StateBag viewState, object key)
        {
            object value = null;

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                value = viewState[key.ToString()];
            }
            return value;
        }


        /// <summary>
        /// Gets a ViewState value as a typed object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as a typed object.</returns>
        public static T GetObject<T>(this StateBag viewState, object key)
        {
            T value = default(T);

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                value = (T)viewState[key.ToString()];
            }
            return value;
        }


        /// <summary>
        /// Gets a ViewState value as a string.
        /// </summary>
        /// <param name="viewState">ViewState.</param>
        /// <param name="key">Key of the ViewState item.</param>
        /// <returns>Value of the ViewState item as a string.</returns>
        public static string GetString(this StateBag viewState, object key)
        {
            string value = null;

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (viewState[key.ToString()] != null)
            {
                value = viewState[key.ToString()].ToString();
            }
            return value;
        }
    }
}

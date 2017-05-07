using System.Collections.Generic;

namespace ReEvolveCSharpLibrary.Containers
{
    /// <summary>
	/// Paged results entity.
	/// </summary>
	/// <typeparam name="T">Type of the results.</typeparam>
	public class PagedResults<T>
    {
        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResults{T}"/> class.
        /// </summary>
        public PagedResults()
        {
            this.Results = new List<T>();
        }

        #endregion

        #region Properties.

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>Current page.</value>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the collection of results.
        /// </summary>
        /// <value>Results.</value>
        public List<T> Results { get; set; }

        /// <summary>
        /// Gets or sets the total number of records available.
        /// </summary>
        /// <value>The total number of records.</value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages available.
        /// </summary>
        /// <value>The total number of pages.</value>
        public int TotalPages { get; set; }

        #endregion
    }
}

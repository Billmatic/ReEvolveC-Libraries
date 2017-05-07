using System;

namespace ReEvolveCSharpLibrary.ErrorHandling
{
    /// <summary>
	/// Provides a generic version of the <see cref="RetryPolicy"/> class.
	/// </summary>
	/// <typeparam name="T">The type implementing the <see cref="ITransientErrorDetectionStrategy"/> 
	/// interface that is responsible for detecting transient conditions.</typeparam>
	public class RetryPolicy<T> : RetryPolicy where T : ITransientErrorDetectionStrategy, new()
    {
        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class.
        /// </summary>
        /// <param name="retryStrategy">The retry strategy to use for this retry policy.</param>
        public RetryPolicy(RetryStrategy retryStrategy)
            : base(new T(), retryStrategy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class.
        /// </summary>
        /// <param name="retryCount">The number of retry attempts.</param>
        public RetryPolicy(int retryCount)
            : base(new T(), retryCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class.
        /// </summary>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="retryInterval">The interval between retries.</param>
        public RetryPolicy(int retryCount, TimeSpan retryInterval)
            : base(new T(), retryCount, retryInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class.
        /// </summary>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="minBackoff">The minimum back-off time.</param>
        /// <param name="maxBackoff">The maximum back-off time.</param>
        /// <param name="deltaBackoff">The time value that will be used for calculating a random delta in the
        /// exponential delay between retries.</param>
        public RetryPolicy(int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)
            : base(new T(), retryCount, minBackoff, maxBackoff, deltaBackoff)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class.
        /// </summary>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="initialInterval">The initial interval that will apply for the first retry.</param>
        /// <param name="increment">The incremental time value that will be used for calculating the 
        /// progressive delay between retries.</param>
        public RetryPolicy(int retryCount, TimeSpan initialInterval, TimeSpan increment)
            : base(new T(), retryCount, initialInterval, increment)
        {
        }

        #endregion
    }
}

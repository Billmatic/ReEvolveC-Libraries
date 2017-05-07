using System;

namespace ReEvolveCSharpLibrary.ErrorHandling
{
    /// <summary>
	/// Contains information required for the <see cref="RetryPolicy.Retrying"/> event.
	/// </summary>
	public class RetryingEventArgs : EventArgs
    {
        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryingEventArgs"/> class.
        /// </summary>
        /// <param name="currentRetryCount">The current retry attempt count.</param>
        /// <param name="delay">The delay indicating how long the current thread will be suspended for 
        /// before the next iteration will be invoked.</param>
        /// <param name="lastException">The exception which caused the retry conditions to occur.</param>
        public RetryingEventArgs(int currentRetryCount, TimeSpan delay, Exception lastException)
        {
            #region Validate arguments.

            if (lastException == null)
            {
                throw new ArgumentNullException("lastException");
            }

            #endregion

            this.CurrentRetryCount = currentRetryCount;
            this.Delay = delay;
            this.LastException = lastException;
        }

        #endregion

        #region Properties.

        /// <summary>
        /// Gets the current retry count.
        /// </summary>
        /// <value>Current retry count.</value>
        public int CurrentRetryCount { get; private set; }

        /// <summary>
        /// Gets the delay indicating how long the current thread will be suspended for before the next 
        /// iteration will be invoked.
        /// </summary>
        /// <value>Delay.</value>
        public TimeSpan Delay { get; private set; }

        /// <summary>
        /// Gets the exception which caused the retry conditions to occur.
        /// </summary>
        /// <value>The last exception.</value>
        public Exception LastException { get; private set; }

        #endregion
    }
}

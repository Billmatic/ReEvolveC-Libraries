using System;
using System.Threading;

namespace ReEvolveCSharpLibrary.ErrorHandling
{
    /// <summary>
	/// Provides the base implementation of the retry mechanism for unreliable actions and transient conditions.
	/// </summary>
	public class RetryPolicy
    {
        #region Public variables.

        /// <summary>
        /// Returns a default policy that does no retries, it just invokes action exactly once.
        /// </summary>
        public static readonly RetryPolicy NoRetry = new RetryPolicy<TransientErrorIgnoreStrategy>(0);

        #endregion

        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the RetryPolicy class with the specified number of retry attempts
        /// and parameters defining the progressive delay between retries.
        /// </summary>
        /// <param name="errorDetectionStrategy">The <see cref="ITransientErrorDetectionStrategy"/> that is 
        /// responsible for detecting transient conditions.</param>
        /// <param name="retryStrategy">The retry strategy to use for this retry policy.</param>
        public RetryPolicy(ITransientErrorDetectionStrategy errorDetectionStrategy, RetryStrategy retryStrategy)
        {
            #region Validate arguments.

            if (errorDetectionStrategy == null)
            {
                throw new ArgumentNullException("errorDetectionStrategy");
            }

            if (retryStrategy == null)
            {
                throw new ArgumentNullException("retryStrategy");
            }

            #endregion

            this.ErrorDetectionStrategy = errorDetectionStrategy;

            if (errorDetectionStrategy == null)
            {
                throw new InvalidOperationException(
                    "The error detection strategy type must implement the ITransientErrorDetectionStrategy interface.");
            }

            this.RetryStrategy = retryStrategy;
        }

        /// <summary>
        /// Initializes a new instance of the RetryPolicy class with the specified number of retry attempts 
        /// and default fixed time interval between retries.
        /// </summary>
        /// <param name="errorDetectionStrategy">The <see cref="ITransientErrorDetectionStrategy"/> that is
        /// responsible for detecting transient conditions.</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        public RetryPolicy(ITransientErrorDetectionStrategy errorDetectionStrategy, int retryCount)
            : this(errorDetectionStrategy, new FixedInterval(retryCount))
        {
        }

        /// <summary>
        /// Initializes a new instance of the RetryPolicy class with the specified number of retry attempts
        /// and fixed time interval between retries.
        /// </summary>
        /// <param name="errorDetectionStrategy">The <see cref="ITransientErrorDetectionStrategy"/> that is
        /// responsible for detecting transient conditions.</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="retryInterval">The interval between retries.</param>
        public RetryPolicy(ITransientErrorDetectionStrategy errorDetectionStrategy, int retryCount,
            TimeSpan retryInterval)
            : this(errorDetectionStrategy, new FixedInterval(retryCount, retryInterval))
        {
        }

        /// <summary>
        /// Initializes a new instance of the RetryPolicy class with the specified number of retry attempts
        /// and back-off parameters for calculating the exponential delay between retries.
        /// </summary>
        /// <param name="errorDetectionStrategy">The <see cref="ITransientErrorDetectionStrategy"/> that is
        /// responsible for detecting transient conditions.</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="minBackoff">The minimum back-off time.</param>
        /// <param name="maxBackoff">The maximum back-off time.</param>
        /// <param name="deltaBackoff">The time value that will be used for calculating a random delta in the
        /// exponential delay between retries.</param>
        public RetryPolicy(ITransientErrorDetectionStrategy errorDetectionStrategy, int retryCount,
            TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)
            : this(errorDetectionStrategy, new ExponentialBackoff(retryCount, minBackoff, maxBackoff,
                deltaBackoff))
        {
        }

        /// <summary>
        /// Initializes a new instance of the RetryPolicy class with the specified number of retry attempts
        /// and parameters defining the progressive delay between retries.
        /// </summary>
        /// <param name="errorDetectionStrategy">The <see cref="ITransientErrorDetectionStrategy"/> that is 
        /// responsible for detecting transient conditions.</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="initialInterval">The initial interval that will apply for the first retry.</param>
        /// <param name="increment">The incremental time value that will be used for calculating the 
        /// progressive delay between retries.</param>
        public RetryPolicy(ITransientErrorDetectionStrategy errorDetectionStrategy, int retryCount,
            TimeSpan initialInterval, TimeSpan increment)
            : this(errorDetectionStrategy, new Incremental(retryCount, initialInterval, increment))
        {
        }

        #endregion

        #region Events.

        /// <summary>
        /// An instance of a callback delegate that will be invoked whenever a retry condition is encountered.
        /// </summary>
        public event EventHandler<RetryingEventArgs> Retrying;

        #endregion

        #region Properties.

        /// <summary>
        /// Gets the retry strategy.
        /// </summary>
        /// <value>Retry strategy.</value>
        public RetryStrategy RetryStrategy { get; private set; }

        /// <summary>
        /// Gets the instance of the error detection strategy.
        /// </summary>
        /// <value>Error detection strategy.</value>
        public ITransientErrorDetectionStrategy ErrorDetectionStrategy { get; private set; }

        #endregion

        #region Public methods.

        /// <summary>
        /// Repetitively executes the specified action while it satisfies the current retry policy.
        /// </summary>
        /// <param name="action">A delegate representing the executable action which doesn't return any
        /// results.</param>
        public virtual void ExecuteAction(Action action)
        {
            #region Validate arguments.

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            #endregion

            this.ExecuteAction(() => { action(); return default(object); });
        }

        /// <summary>
        /// Repetitively executes the specified action while it satisfies the current retry policy.
        /// </summary>
        /// <typeparam name="TResult">The type of result expected from the executable action.</typeparam>
        /// <param name="func">A delegate representing the executable action which returns the result of type R.</param>
        /// <returns>The result from the action.</returns>
        public virtual TResult ExecuteAction<TResult>(Func<TResult> func)
        {
            #region Validate arguments.

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            #endregion

            int retryCount = 0;
            TimeSpan delay = TimeSpan.Zero;
            Exception lastError;

            var shouldRetry = this.RetryStrategy.GetShouldRetry();

            for (;;)
            {
                lastError = null;

                try
                {
                    return func();
                }
                catch (RetryLimitExceededException limitExceededEx)
                {
                    // The user code can throw a RetryLimitExceededException to force the exit from the 
                    // retry loop. The RetryLimitExceeded exception can have an inner exception attached to
                    // it. This is the exception which we will have to throw up the stack so that callers 
                    // can handle it.
                    if (limitExceededEx.InnerException != null)
                    {
                        throw limitExceededEx.InnerException;
                    }
                    else
                    {
                        return default(TResult);
                    }
                }
                catch (Exception ex)
                {
                    lastError = ex;

                    if (!(this.ErrorDetectionStrategy.IsTransient(lastError) &&
                        shouldRetry(retryCount++, lastError, out delay)))
                    {
                        throw;
                    }
                }

                // Perform an extra check in the delay interval. Should prevent from accidentally ending up 
                // with the value of -1 that will block a thread indefinitely. In addition, any other 
                // negative numbers will cause an ArgumentOutOfRangeException fault that will be thrown by
                // Thread.Sleep.
                if (delay.TotalMilliseconds < 0)
                {
                    delay = TimeSpan.Zero;
                }

                this.OnRetrying(retryCount, lastError, delay);

                if (retryCount > 1 || !this.RetryStrategy.FastFirstRetry)
                {
                    Thread.Sleep(delay);
                }
            }
        }

        #endregion

        #region Protected methods.

        /// <summary>
        /// Notifies the subscribers whenever a retry condition is encountered.
        /// </summary>
        /// <param name="retryCount">The current retry attempt count.</param>
        /// <param name="lastError">The exception which caused the retry conditions to occur.</param>
        /// <param name="delay">The delay indicating how long the current thread will be suspended for 
        /// before the next iteration will be invoked.</param>
        protected virtual void OnRetrying(int retryCount, Exception lastError, TimeSpan delay)
        {
            if (this.Retrying != null)
            {
                this.Retrying(this, new RetryingEventArgs(retryCount, delay, lastError));
            }
        }

        #endregion

        #region Private classes.

        /// <summary>
        /// Implements a strategy that ignores any transient errors.
        /// </summary>
        private sealed class TransientErrorIgnoreStrategy : ITransientErrorDetectionStrategy
        {
            /// <summary>
            /// Always return false.
            /// </summary>
            /// <param name="ex">The exception.</param>
            /// <returns>Returns false.</returns>
            public bool IsTransient(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Implements a strategy that treats all exceptions as transient errors.
        /// </summary>
        private sealed class TransientErrorCatchAllStrategy : ITransientErrorDetectionStrategy
        {
            /// <summary>
            /// Always return true.
            /// </summary>
            /// <param name="ex">The exception.</param>
            /// <returns>Returns true.</returns>
            public bool IsTransient(Exception ex)
            {
                return true;
            }
        }

        #endregion
    }
}

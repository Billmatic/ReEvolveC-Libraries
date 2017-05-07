using System;

namespace ReEvolveCSharpLibrary.ErrorHandling
{
    /// <summary>
	/// A retry strategy with a specified number of retry attempts and a default fixed time interval between retries.
	/// </summary>
	public class FixedInterval : RetryStrategy
    {
        #region Private variables.

        /// <summary>
        /// The number of retry attempts.
        /// </summary>
        private readonly int _retryCount;

        /// <summary>
        /// The time interval between retries.
        /// </summary>
        private readonly TimeSpan _retryInterval;

        #endregion

        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedInterval"/> class. 
        /// </summary>
        public FixedInterval()
            : this(DefaultClientRetryCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedInterval"/> class. 
        /// </summary>
        /// <param name="retryCount">The number of retry attempts.</param>
        public FixedInterval(int retryCount)
            : this(retryCount, DefaultRetryInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedInterval"/> class. 
        /// </summary>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="retryInterval">The time interval between retries.</param>
        public FixedInterval(int retryCount, TimeSpan retryInterval)
            : this(null, retryCount, retryInterval, DefaultFirstFastRetry)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedInterval"/> class. 
        /// </summary>
        /// <param name="name">The retry strategy name.</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="retryInterval">The time interval between retries.</param>
        public FixedInterval(string name, int retryCount, TimeSpan retryInterval)
            : this(name, retryCount, retryInterval, DefaultFirstFastRetry)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedInterval"/> class. 
        /// </summary>
        /// <param name="name">The retry strategy name.</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="retryInterval">The time interval between retries.</param>
        /// <param name="firstFastRetry">A value indicating whether or not the very first retry attempt will 
        /// be made immediately whereas the subsequent retries will remain subject to the retry 
        /// interval.</param>
        public FixedInterval(string name, int retryCount, TimeSpan retryInterval, bool firstFastRetry)
            : base(name, firstFastRetry)
        {
            #region Validate arguments.

            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException("retryCount", retryCount,
                    "The specified argument retryCount cannot be initialized with a negative value.");
            }

            if (retryInterval.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException("retryInterval", retryInterval.Ticks,
                    "The specified argument retryInterval cannot be initialized with a negative value.");
            }

            #endregion

            _retryCount = retryCount;
            _retryInterval = retryInterval;
        }

        #endregion

        #region Public methods.

        /// <summary>
        /// Returns the corresponding ShouldRetry delegate.
        /// </summary>
        /// <returns>The ShouldRetry delegate.</returns>
        public override ShouldRetry GetShouldRetry()
        {
            if (_retryCount == 0)
            {
                return delegate (int currentRetryCount, Exception lastException, out TimeSpan interval)
                {
                    interval = TimeSpan.Zero;
                    return false;
                };
            }

            return delegate (int currentRetryCount, Exception lastException, out TimeSpan interval)
            {
                if (currentRetryCount < _retryCount)
                {
                    interval = _retryInterval;
                    return true;
                }

                interval = TimeSpan.Zero;
                return false;
            };
        }

        #endregion
    }
}

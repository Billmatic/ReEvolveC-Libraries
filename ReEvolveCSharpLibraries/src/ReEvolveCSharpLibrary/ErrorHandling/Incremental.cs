using System;

namespace ReEvolveCSharpLibrary.ErrorHandling
{
    /// <summary>
	/// A retry strategy with a specified number of retry attempts and an incremental time interval between
	/// retries.
	/// </summary>
	public class Incremental : RetryStrategy
    {
        #region Private variables.

        /// <summary>
        /// The incremental time value that will be used for calculating the progressive delay between 
        /// retries.
        /// </summary>
        private readonly TimeSpan _increment;

        /// <summary>
        /// The initial interval that will apply for the first retry.
        /// </summary>
        private readonly TimeSpan _initialInterval;

        /// <summary>
        /// The number of retry attempts.
        /// </summary>
        private readonly int _retryCount;

        #endregion

        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the <see cref="Incremental"/> class. 
        /// </summary>
        public Incremental()
            : this(DefaultClientRetryCount, DefaultRetryInterval, DefaultRetryIncrement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Incremental"/> class. 
        /// </summary>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="initialInterval">The initial interval that will apply for the first retry.</param>
        /// <param name="increment">The incremental time value that will be used for calculating the 
        /// progressive delay between retries.</param>
        public Incremental(int retryCount, TimeSpan initialInterval, TimeSpan increment)
            : this(null, retryCount, initialInterval, increment)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Incremental"/> class. 
        /// </summary>
        /// <param name="name">The retry strategy name.</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="initialInterval">The initial interval that will apply for the first retry.</param>
        /// <param name="increment">The incremental time value that will be used for calculating the 
        /// progressive delay between retries.</param>
        public Incremental(string name, int retryCount, TimeSpan initialInterval, TimeSpan increment)
            : this(name, retryCount, initialInterval, increment, DefaultFirstFastRetry)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Incremental"/> class. 
        /// </summary>
        /// <param name="name">The retry strategy name.</param>
        /// <param name="retryCount">The number of retry attempts.</param>
        /// <param name="initialInterval">The initial interval that will apply for the first retry.</param>
        /// <param name="increment">The incremental time value that will be used for calculating the 
        /// progressive delay between retries.</param>
        /// <param name="firstFastRetry">A value indicating whether or not the very first retry attempt
        /// will be made immediately whereas the subsequent retries will remain subject to the retry 
        /// interval.</param>
        public Incremental(string name, int retryCount, TimeSpan initialInterval, TimeSpan increment,
            bool firstFastRetry)
            : base(name, firstFastRetry)
        {
            #region Validate arguments.

            if (increment.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException("increment", increment,
                    "The specified argument increment cannot be initialized with a negative value.");
            }

            if (initialInterval.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException("initialInterval", initialInterval.Ticks,
                    "The specified argument initialInterval cannot be initialized with a negative value.");
            }

            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException("retryCount", retryCount,
                    "The specified argument retryCount cannot be initialized with a negative value.");
            }

            #endregion

            _increment = increment;
            _initialInterval = initialInterval;
            _retryCount = retryCount;
        }

        #endregion

        #region Public methods.

        /// <summary>
        /// Returns the corresponding ShouldRetry delegate.
        /// </summary>
        /// <returns>The ShouldRetry delegate.</returns>
        public override ShouldRetry GetShouldRetry()
        {
            return delegate (int currentRetryCount, Exception lastException, out TimeSpan retryInterval)
            {
                if (currentRetryCount < _retryCount)
                {
                    retryInterval = TimeSpan.FromMilliseconds(_initialInterval.TotalMilliseconds +
                        (_increment.TotalMilliseconds * currentRetryCount));

                    return true;
                }

                retryInterval = TimeSpan.Zero;

                return false;
            };
        }

        #endregion
    }
}

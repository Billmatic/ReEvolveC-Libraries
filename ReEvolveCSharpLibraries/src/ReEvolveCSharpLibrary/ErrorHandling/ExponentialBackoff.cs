using System;

namespace ReEvolveCSharpLibrary.ErrorHandling
{
    /// <summary>
    /// A retry strategy with a specified number of retry attempts and an exponential time interval between 
    /// retries.
    /// </summary>
    public class ExponentialBackoff : RetryStrategy
    {
        #region Private variables.

        /// <summary>
        /// The value that will be used for calculating a random delta in the exponential delay between
        /// retries.
        /// </summary>
        private readonly TimeSpan _deltaBackoff;

        /// <summary>
        /// The maximum back-off time.
        /// </summary>
        private readonly TimeSpan _maxBackoff;

        /// <summary>
        /// The minimum back-off time.
        /// </summary>
        private readonly TimeSpan _minBackoff;

        /// <summary>
        /// The number of retry attempts.
        /// </summary>
        private readonly int _retryCount;

        #endregion

        #region Constructors.

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialBackoff"/> class. 
        /// </summary>
        public ExponentialBackoff()
            : this(DefaultClientRetryCount, DefaultMinBackoff, DefaultMaxBackoff, DefaultClientBackoff)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialBackoff"/> class. 
        /// </summary>
        /// <param name="retryCount">The maximum number of retry attempts.</param>
        /// <param name="minBackoff">The minimum back-off time.</param>
        /// <param name="maxBackoff">The maximum back-off time.</param>
        /// <param name="deltaBackoff">The value that will be used for calculating a random delta in the
        /// exponential delay between retries.</param>
        public ExponentialBackoff(int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff,
            TimeSpan deltaBackoff)
            : this(null, retryCount, minBackoff, maxBackoff, deltaBackoff, DefaultFirstFastRetry)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialBackoff"/> class. 
        /// </summary>
        /// <param name="name">The name of the retry strategy.</param>
        /// <param name="retryCount">The maximum number of retry attempts.</param>
        /// <param name="minBackoff">The minimum back-off time.</param>
        /// <param name="maxBackoff">The maximum back-off time.</param>
        /// <param name="deltaBackoff">The value that will be used for calculating a random delta in the
        /// exponential delay between retries.</param>
        public ExponentialBackoff(string name, int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff,
            TimeSpan deltaBackoff)
            : this(name, retryCount, minBackoff, maxBackoff, deltaBackoff, DefaultFirstFastRetry)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialBackoff"/> class. 
        /// </summary>
        /// <param name="name">The name of the retry strategy.</param>
        /// <param name="retryCount">The maximum number of retry attempts.</param>
        /// <param name="minBackoff">The minimum back-off time.</param>
        /// <param name="maxBackoff">The maximum back-off time.</param>
        /// <param name="deltaBackoff">The value that will be used for calculating a random delta in the 
        /// exponential delay between retries.</param>
        /// <param name="firstFastRetry">A value indicating whether or not the very first retry attempt will
        /// be made immediately whereas the subsequent retries will remain subject to the retry 
        /// interval.</param>
        public ExponentialBackoff(string name, int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff,
            TimeSpan deltaBackoff, bool firstFastRetry)
            : base(name, firstFastRetry)
        {
            #region Validate arguments.

            if (deltaBackoff.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException("deltaBackoff", deltaBackoff.Ticks,
                    "The specified argument deltaBackoff cannot be initialized with a negative value.");
            }

            if (maxBackoff.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException("maxBackoff", maxBackoff.Ticks,
                    "The specified argument maxBackoff cannot be initialized with a negative value.");
            }

            if (minBackoff.Ticks < 0)
            {
                throw new ArgumentOutOfRangeException("minBackoff", minBackoff.Ticks,
                    "The specified argument minBackoff cannot be initialized with a negative value.");
            }

            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException("retryCount", retryCount,
                    "The specified argument retryCount cannot be initialized with a negative value.");
            }

            if (minBackoff.TotalMilliseconds > maxBackoff.TotalMilliseconds)
            {
                string message = string.Format(
                    "The specified argument minBackoff cannot be greater than its ceiling value of {0}",
                    maxBackoff.TotalMilliseconds);

                throw new ArgumentOutOfRangeException("minBackoff", minBackoff.TotalMilliseconds,
                    message);
            }

            #endregion

            _deltaBackoff = deltaBackoff;
            _maxBackoff = maxBackoff;
            _minBackoff = minBackoff;
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
                    var random = new Random();

                    var delta = (int)((Math.Pow(2.0, currentRetryCount) - 1.0) *
                        random.Next((int)(_deltaBackoff.TotalMilliseconds * 0.8),
                        (int)(_deltaBackoff.TotalMilliseconds * 1.2)));
                    var interval = (int)Math.Min(checked(_minBackoff.TotalMilliseconds + delta),
                        _maxBackoff.TotalMilliseconds);

                    retryInterval = TimeSpan.FromMilliseconds(interval);

                    return true;
                }

                retryInterval = TimeSpan.Zero;
                return false;
            };
        }

        #endregion
    }
}

using System;
using System.Runtime.Serialization;

namespace ReEvolveCSharpLibrary.ErrorHandling
{
    /// <summary>
	/// The special type of exception that provides a managed exit from a retry loop. User code can use this
	/// exception to notify the retry policy that no further retry attempts are required.
	/// </summary>
	[Serializable]
    public sealed class RetryLimitExceededException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RetryLimitExceededException"/> class.
        /// </summary>
        public RetryLimitExceededException()
            : this("The action has exceeded its defined retry limit.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryLimitExceededException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RetryLimitExceededException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryLimitExceededException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RetryLimitExceededException(Exception innerException)
            : base(innerException != null ? innerException.Message :
                "The action has exceeded its defined retry limit.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryLimitExceededException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RetryLimitExceededException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryLimitExceededException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that 
        /// contains contextual information about the source or destination.</param>
        private RetryLimitExceededException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

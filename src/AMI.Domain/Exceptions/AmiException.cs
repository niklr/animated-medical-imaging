using System;

namespace AMI.Domain.Exceptions
{
    /// <summary>
    /// This exception is thrown when an exception specific to the AMI application occurs.
    /// </summary>
    public class AmiException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmiException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public AmiException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets or sets the source of the exception.
        /// </summary>
        public override string Source
        {
            get { return "AMI"; }
            set { base.Source = value; }
        }
    }
}

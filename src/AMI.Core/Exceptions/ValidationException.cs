using System;
using System.Collections.Generic;

namespace PNL.Application.Exceptions
{
    /// <summary>
    /// This exception is thrown when an exception related to validation occurs.
    /// </summary>
    /// <seealso cref="Exception" />
    public class ValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Gets the validation failures.
        /// </summary>
        public IDictionary<string, string[]> Failures { get; }
    }
}

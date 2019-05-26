using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace AMI.Core.Exceptions
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
        /// Initializes a new instance of the <see cref="ValidationException" /> class.
        /// </summary>
        /// <param name="failures">The validation failures.</param>
        /// <exception cref="ArgumentNullException">failures</exception>
        public ValidationException(List<ValidationFailure> failures)
            : this()
        {
            if (failures == null)
            {
                throw new ArgumentNullException(nameof(failures));
            }

            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        /// <summary>
        /// Gets the validation failures.
        /// </summary>
        public IDictionary<string, string[]> Failures { get; }
    }
}

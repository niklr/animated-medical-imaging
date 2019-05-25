using System.Collections.Generic;

namespace AMI.Core.Entities.ViewModels
{
    /// <summary>
    /// A model representing an error result.
    /// </summary>
    public class ErrorResult
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        public IDictionary<string, string[]> ValidationErrors { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace { get; set; }
    }
}

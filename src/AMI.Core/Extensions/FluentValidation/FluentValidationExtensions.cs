using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace AMI.Core.Extensions.FluentValidationExtensions
{
    /// <summary>
    /// Extensions related to FluentValidation.
    /// </summary>
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// Converts the provided list of validation failures to a dictionary.
        /// </summary>
        /// <param name="failures">The validation failures.</param>
        /// <returns>A dictionary based on the provided list of validation failures.</returns>
        public static IDictionary<string, string[]> ToDictionary(this IList<ValidationFailure> failures)
        {
            IDictionary<string, string[]> dictionary = new Dictionary<string, string[]>();

            try
            {
                if (failures == null || failures.Count <= 0)
                {
                    return dictionary;
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

                    dictionary.Add(propertyName, propertyFailures);
                }

                return dictionary;
            }
            catch (Exception)
            {
                return dictionary;
            }
        }
    }
}

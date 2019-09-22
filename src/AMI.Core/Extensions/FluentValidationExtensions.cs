using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
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

        /// <summary>
        /// The value of the property must be one of the provided options.
        /// </summary>
        /// <typeparam name="T">The type of object being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="validOptions">The valid options.</param>
        /// <returns>The rule builder options.</returns>
        /// <exception cref="ArgumentException">At least one valid option is expected - validOptions</exception>
        public static IRuleBuilderOptions<T, TProperty> In<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, params TProperty[] validOptions)
        {
            string formatted;
            if (validOptions == null || validOptions.Length == 0)
            {
                throw new ArgumentException("At least one valid option is expected", nameof(validOptions));
            }
            else if (validOptions.Length == 1)
            {
                formatted = validOptions[0].ToString();
            }
            else
            {
                // format like: option1, option2 or option3
                formatted = $"{string.Join(", ", validOptions.Select(vo => vo.ToString()).ToArray(), 0, validOptions.Length - 1)} or {validOptions.Last()}";
            }

            return ruleBuilder
                .Must(validOptions.Contains)
                .WithMessage($"'{{PropertyName}}' must be one of these values: {formatted}");
        }

        /// <summary>
        /// Applies the GUID validation.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="rule">The rule builder.</param>
        /// <returns>The rule builder options.</returns>
        public static IRuleBuilderOptions<T, string> GuidValidation<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .Must(x =>
                {
                    return Guid.TryParse(x, out Guid result);
                });
        }

        /// <summary>
        /// Applies the optional GUID validation.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="rule">The rule builder.</param>
        /// <returns>The rule builder options.</returns>
        public static IRuleBuilderOptions<T, string> OptionalGuidValidation<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .Must(x =>
                {
                    if (string.IsNullOrWhiteSpace(x))
                    {
                        return true;
                    }
                    return Guid.TryParse(x, out Guid result);
                });
        }

        /// <summary>
        /// Applies the URL validation.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated.</typeparam>
        /// <param name="rule">The rule builder.</param>
        /// <returns>The rule builder options.</returns>
        public static IRuleBuilderOptions<T, string> UrlValidation<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .Must(x =>
                {
                    return Uri.TryCreate(x, UriKind.Absolute, out Uri uri)
                            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
                });
        }
    }
}

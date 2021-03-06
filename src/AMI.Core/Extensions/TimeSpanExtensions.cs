﻿using System;
using System.Collections.Generic;

namespace AMI.Core.Extensions.TimeSpanExtensions
{
    /// <summary>
    /// Extensions related to time spans.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Converts the provided time span to a readable format.
        /// </summary>
        /// <param name="t">The time span.</param>
        /// <returns>The time span in a readable format.</returns>
        /// <exception cref="ArgumentNullException">t</exception>
        public static string ToReadableTime(this TimeSpan t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            var parts = new List<string>();
            Action<int, string> add = (val, unit) =>
            {
                if (val > 0)
                {
                    parts.Add(val + unit);
                }
            };

            add(t.Days, "d");
            add(t.Hours, "h");
            add(t.Minutes, "m");
            add(t.Seconds, "s");
            add(t.Milliseconds, "ms");

            return string.Join(" ", parts);
        }
    }
}

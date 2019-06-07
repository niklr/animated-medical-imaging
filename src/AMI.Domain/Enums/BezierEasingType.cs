namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the Bézier curve easing.
    /// </summary>
    public enum BezierEasingType
    {
        /// <summary>
        /// The default type.
        /// </summary>
        None = 0,

        /// <summary>
        /// The linear Bézier curve type.
        /// </summary>
        Linear = 1,

        /// <summary>
        /// The ease-in cubic Bézier curve type.
        /// </summary>
        EaseInCubic = 2,

        /// <summary>
        /// The ease-out cubic Bézier curve type.
        /// </summary>
        EaseOutCubic = 3,

        /// <summary>
        /// The ease-in-out cubic Bézier curve type.
        /// </summary>
        EaseInOutCubic = 4,

        /// <summary>
        /// The ease-in quartic Bézier curve type.
        /// </summary>
        EaseInQuart = 5,

        /// <summary>
        /// The ease-out quartic Bézier curve type.
        /// </summary>
        EaseOutQuart = 6,

        /// <summary>
        /// The ease-in-out quartic Bézier curve type.
        /// </summary>
        EaseInOutQuart = 7
    }
}

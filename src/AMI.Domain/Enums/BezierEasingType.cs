namespace AMI.Domain.Enums
{
    /// <summary>
    /// A type to describe the Bézier curve easing.
    /// </summary>
    public enum BezierEasingType
    {
        /// <summary>
        /// The linear Bézier curve type.
        /// </summary>
        Linear,

        /// <summary>
        /// The ease-in cubic Bézier curve type.
        /// </summary>
        EaseInCubic,

        /// <summary>
        /// The ease-out cubic Bézier curve type.
        /// </summary>
        EaseOutCubic,

        /// <summary>
        /// The ease-in-out cubic Bézier curve type.
        /// </summary>
        EaseInOutCubic,

        /// <summary>
        /// The ease-in quartic Bézier curve type.
        /// </summary>
        EaseInQuart,

        /// <summary>
        /// The ease-out quartic Bézier curve type.
        /// </summary>
        EaseOutQuart,

        /// <summary>
        /// The ease-in-out quartic Bézier curve type.
        /// </summary>
        EaseInOutQuart
    }
}

using System;
using System.Numerics;
using AMI.Domain.Enums;

namespace AMI.Core.Mappers
{
    /// <summary>
    /// A mapper for positions based on Bézier curves.
    /// </summary>
    public class BezierPositionMapper
    {
        private readonly Vector2 v0;
        private readonly Vector2 v1;
        private readonly Vector2 v2;
        private readonly Vector2 v3;
        private readonly float[] map;

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierPositionMapper"/> class.
        /// </summary>
        /// <param name="amount">The amount of positions.</param>
        /// <param name="bezierEasingType">Type of the Bézier curve easing.</param>
        public BezierPositionMapper(int amount, BezierEasingType bezierEasingType)
            : this(amount)
        {
            var vectors = ConvertBezierEasingType(bezierEasingType);

            v1 = vectors.Item1;
            v2 = vectors.Item2;

            Init(amount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezierPositionMapper"/> class.
        /// </summary>
        /// <param name="amount">The amount positions.</param>
        /// <param name="x1">The value to assign to the first x field.</param>
        /// <param name="y1">The value to assign to the first y field.</param>
        /// <param name="x2">The value to assign to the second x field.</param>
        /// <param name="y2">The value to assign to the second y field.</param>
        public BezierPositionMapper(int amount, float x1, float y1, float x2, float y2)
            : this(amount)
        {
            v1 = new Vector2(x1, y1);
            v2 = new Vector2(x2, y2);

            Init(amount);
        }

        private BezierPositionMapper(int amount)
        {
            v0 = new Vector2(0, 0);
            v3 = new Vector2(1, 1);

            map = new float[amount];
        }

        /// <summary>
        /// Gets the point on Bézier curve based on the provided time.
        /// Source: https://denisrizov.com/2016/06/02/bezier-curves-unity-package-included/
        /// </summary>
        /// <param name="t">The time.</param>
        /// <returns>The point on the Bézier curve.</returns>
        public Tuple<float, float> GetPointOnBezierCurve(float t)
        {
            float u = 1f - t;
            float t2 = t * t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t3 = t2 * t;

            Vector2 result =
                (u3 * v0) +
                (3f * u2 * t * v1) +
                (3f * u * t2 * v2) +
                (t3 * v3);

            return new Tuple<float, float>(result.X, result.Y);
        }

        /// <summary>
        /// Gets the mapped position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The mapped position.</returns>
        public float GetMappedPosition(int position)
        {
            if (map != null && position < map.Length)
            {
                return map[position];
            }

            return default;
        }

        private Tuple<Vector2, Vector2> ConvertBezierEasingType(BezierEasingType bezierEasingType)
        {
            switch (bezierEasingType)
            {
                case BezierEasingType.EaseInCubic:
                    return new Tuple<Vector2, Vector2>(new Vector2(0.55f, 0.055f), new Vector2(0.675f, 0.19f));
                case BezierEasingType.EaseOutCubic:
                    return new Tuple<Vector2, Vector2>(new Vector2(0.215f, 0.61f), new Vector2(0.355f, 1f));
                case BezierEasingType.EaseInOutCubic:
                    return new Tuple<Vector2, Vector2>(new Vector2(0.645f, 0.045f), new Vector2(0.355f, 1f));
                case BezierEasingType.EaseInQuart:
                    return new Tuple<Vector2, Vector2>(new Vector2(0.895f, 0.03f), new Vector2(0.685f, 0.22f));
                case BezierEasingType.EaseOutQuart:
                    return new Tuple<Vector2, Vector2>(new Vector2(0.165f, 0.84f), new Vector2(0.44f, 1f));
                case BezierEasingType.EaseInOutQuart:
                    return new Tuple<Vector2, Vector2>(new Vector2(0.77f, 0f), new Vector2(0.175f, 1f));
                case BezierEasingType.Linear:
                default:
                    break;
            }

            return new Tuple<Vector2, Vector2>(new Vector2(0.25f, 0.25f), new Vector2(0.75f, 0.75f));
        }

        private Vector2 GetPointOnBezierCurvePrivate(float t)
        {
            var result = GetPointOnBezierCurve(t);
            return new Vector2(result.Item1, result.Item2);
        }

        private void Init(int amount)
        {
            /*
             * fps = 30
             * overall duration = 1000ms
             * duration/frame = 33ms
             */

            if (amount > 0)
            {
                // 33ms delay (~30fps)
                // 50ms delay (~20fps)
                // 66ms delay (~15fps)
                // 100ms delay (~10fps)
                float fps;
                switch (amount)
                {
                    case int a when a <= 10:
                        fps = 10f;
                        break;
                    case int a when a <= 40:
                        fps = 15f;
                        break;
                    default:
                        fps = 20f;
                        break;
                }

                // overall duration
                float duration = amount / fps * 1000;

                // average delay
                float averageDelay = duration / amount;

                var initialPoint = GetPointOnBezierCurvePrivate(averageDelay / duration);
                map[0] = initialPoint.Y * duration;

                for (int i = 1; i < amount; i++)
                {
                    var previousPoint = GetPointOnBezierCurvePrivate(averageDelay * (i - 1) / duration);
                    var currentPoint = GetPointOnBezierCurvePrivate(averageDelay * i / duration);
                    map[i] = (currentPoint.Y - previousPoint.Y) * duration;
                }
            }
        }
    }
}

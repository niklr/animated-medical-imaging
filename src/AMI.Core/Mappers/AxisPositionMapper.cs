using System;
using System.Collections.Generic;
using AMI.Domain.Enums;

namespace AMI.Core.Mappers
{
    /// <summary>
    /// A mapper for positions of each axis in the coordinate system.
    /// </summary>
    /// <seealso cref="IAxisPositionMapper" />
    public class AxisPositionMapper : IAxisPositionMapper
    {
        private readonly IDictionary<AxisType, int[]> map;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisPositionMapper"/> class.
        /// </summary>
        /// <param name="map">The positions map.</param>
        public AxisPositionMapper(IDictionary<AxisType, int[]> map)
        {
            this.map = map ?? new Dictionary<AxisType, int[]>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisPositionMapper"/> class.
        /// </summary>
        /// <param name="amount">The amount of images.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="depth">The depth of the image.</param>
        public AxisPositionMapper(int amount, int width, int height, int depth)
            : this(new Dictionary<AxisType, int[]>())
        {
            Init(amount, width, height, depth);
        }

        /// <inheritdoc/>
        public int CalculateMappedPosition(int amount, int length, int position)
        {
            int stepSize = Math.Max(length / amount, 1);
            int mappedPosition = position * stepSize;
            if (length == 0 || mappedPosition > length - 1)
            {
                mappedPosition = 0;
            }

            return mappedPosition;
        }

        /// <inheritdoc/>
        public int GetLength(AxisType axisType)
        {
            if (map != null)
            {
                if (map.TryGetValue(axisType, out int[] positions))
                {
                    return positions.Length;
                }
            }

            return 0;
        }

        /// <inheritdoc/>
        public int GetMappedPosition(AxisType axisType, int position)
        {
            if (map != null)
            {
                if (map.TryGetValue(axisType, out int[] positions))
                {
                    if (positions != null && position < positions.Length)
                    {
                        return positions[position];
                    }
                }
            }

            return position;
        }

        private void Init(int amount, int width, int height, int depth)
        {
            foreach (AxisType axisType in (AxisType[])Enum.GetValues(typeof(AxisType)))
            {
                int length = 0;

                switch (axisType)
                {
                    case AxisType.X:
                        length = width;
                        break;
                    case AxisType.Y:
                        length = height;
                        break;
                    case AxisType.Z:
                        length = depth;
                        break;
                    default:
                        break;
                }

                map[axisType] = new int[Math.Min(amount, length)];

                for (int position = 0; position < map[axisType].Length; position++)
                {
                    map[axisType][position] = CalculateMappedPosition(amount, length, position);
                }
            }
        }
    }
}

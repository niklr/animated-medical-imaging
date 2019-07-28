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
        private readonly IDictionary<AxisType, uint[]> map;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisPositionMapper"/> class.
        /// </summary>
        /// <param name="map">The positions map.</param>
        public AxisPositionMapper(IDictionary<AxisType, uint[]> map)
        {
            this.map = map ?? new Dictionary<AxisType, uint[]>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisPositionMapper"/> class.
        /// </summary>
        /// <param name="amount">The amount of images.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="depth">The depth of the image.</param>
        public AxisPositionMapper(uint amount, uint width, uint height, uint depth)
            : this(new Dictionary<AxisType, uint[]>())
        {
            Init(amount, width, height, depth);
        }

        /// <inheritdoc/>
        public uint CalculateMappedPosition(uint amount, uint length, uint position)
        {
            uint stepSize = Math.Max(length / amount, 1);
            uint mappedPosition = position * stepSize;
            if (length == 0 || mappedPosition > length - 1)
            {
                mappedPosition = 0;
            }

            return mappedPosition;
        }

        /// <inheritdoc/>
        public uint GetMappedPosition(AxisType axisType, uint position)
        {
            if (map != null)
            {
                if (map.TryGetValue(axisType, out uint[] positions))
                {
                    if (positions != null && position < positions.Length)
                    {
                        return positions[position];
                    }
                }
            }

            return position;
        }

        private void Init(uint amount, uint width, uint height, uint depth)
        {
            foreach (AxisType axisType in (AxisType[])Enum.GetValues(typeof(AxisType)))
            {
                map[axisType] = new uint[amount];

                uint length = 0;

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

                for (uint position = 0; position < amount; position++)
                {
                    map[axisType][position] = CalculateMappedPosition(amount, length, position);
                }
            }
        }
    }
}

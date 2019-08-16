using AMI.Core.Mappers;
using AMI.Domain.Enums;
using NUnit.Framework;

namespace AMI.NetFramework.Tests.Core.Mappers
{
    [TestFixture]
    public class AxisPositionMapperTests
    {
        [Test]
        public void AxisPositionMapper_Map_1()
        {
            // Arrange
            uint amount = 10;
            uint width = 100;
            uint height = 200;
            uint depth = 300;

            // Act
            var mapper = new AxisPositionMapper(amount, width, height, depth);

            // Assert
            Assert.AreEqual(0, mapper.GetMappedPosition(AxisType.X, 0));
            Assert.AreEqual(10, mapper.GetMappedPosition(AxisType.X, 1));
            Assert.AreEqual(90, mapper.GetMappedPosition(AxisType.X, 9));

            Assert.AreEqual(0, mapper.GetMappedPosition(AxisType.Y, 0));
            Assert.AreEqual(20, mapper.GetMappedPosition(AxisType.Y, 1));
            Assert.AreEqual(180, mapper.GetMappedPosition(AxisType.Y, 9));

            Assert.AreEqual(0, mapper.GetMappedPosition(AxisType.Z, 0));
            Assert.AreEqual(30, mapper.GetMappedPosition(AxisType.Z, 1));
            Assert.AreEqual(270, mapper.GetMappedPosition(AxisType.Z, 9));
        }

        [Test]
        public void AxisPositionMapper_Map_2()
        {
            // Arrange
            uint amount = 10;
            uint width = 5;
            uint height = 10;
            uint depth = 0;

            // Act
            var mapper = new AxisPositionMapper(amount, width, height, depth);

            // Assert
            Assert.AreEqual(0, mapper.GetMappedPosition(AxisType.X, 0));
            Assert.AreEqual(1, mapper.GetMappedPosition(AxisType.X, 1));
            Assert.AreEqual(4, mapper.GetMappedPosition(AxisType.X, 4));
            Assert.AreEqual(5, mapper.GetMappedPosition(AxisType.X, 5));
            Assert.AreEqual(9, mapper.GetMappedPosition(AxisType.X, 9));
            Assert.AreEqual(20, mapper.GetMappedPosition(AxisType.X, 20));

            Assert.AreEqual(0, mapper.GetMappedPosition(AxisType.Y, 0));
            Assert.AreEqual(1, mapper.GetMappedPosition(AxisType.Y, 1));
            Assert.AreEqual(9, mapper.GetMappedPosition(AxisType.Y, 9));

            Assert.AreEqual(0, mapper.GetMappedPosition(AxisType.Z, 0));
            Assert.AreEqual(1, mapper.GetMappedPosition(AxisType.Z, 1));
            Assert.AreEqual(9, mapper.GetMappedPosition(AxisType.Z, 9));
        }
    }
}

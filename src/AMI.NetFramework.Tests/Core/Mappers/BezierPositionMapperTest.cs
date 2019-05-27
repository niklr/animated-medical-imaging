using System;
using AMI.Core.Mappers;
using AMI.Domain.Enums;
using NUnit.Framework;

namespace AMI.NetFramework.Tests.Core.Mappers
{
    [TestFixture]
    public class BezierPositionMapperTest
    {
        [Test]
        public void BezierPositionMapper_GetPointOnBezierCurve()
        {
            // Arrange
            uint amount = 60;
            float x1 = 0.25f;
            float y1 = 0.25f;
            float x2 = 0.75f;
            float y2 = 0.75f;

            // Act
            var mapper = new BezierPositionMapper(amount, x1, y1, x2, y2);

            // Assert
            Assert.AreEqual(new Tuple<float, float>(0, 0), mapper.GetPointOnBezierCurve(0));
            Assert.AreEqual(new Tuple<float, float>(1, 1), mapper.GetPointOnBezierCurve(1));
            Assert.AreEqual(new Tuple<float, float>(0.5f, 0.5f), mapper.GetPointOnBezierCurve(0.5f));

            float duration = 0;
            for (uint i = 0; i < amount; i++)
            {
                duration += mapper.GetMappedPosition(i);
            }
            Assert.AreEqual(3000, Convert.ToInt32(duration));
        }

        [Test]
        public void BezierPositionMapper_GetPointOnBezierCurve_BezierEasingType()
        {
            // Arrange
            uint amount = 60;
            BezierEasingType bezierEasingType = BezierEasingType.Linear;

            // Act
            var mapper = new BezierPositionMapper(amount, bezierEasingType);

            // Assert
            Assert.AreEqual(new Tuple<float, float>(0, 0), mapper.GetPointOnBezierCurve(0));
            Assert.AreEqual(new Tuple<float, float>(1, 1), mapper.GetPointOnBezierCurve(1));
            Assert.AreEqual(new Tuple<float, float>(0.5f, 0.5f), mapper.GetPointOnBezierCurve(0.5f));

            float duration = 0;
            for (uint i = 0; i < amount; i++)
            {
                duration += mapper.GetMappedPosition(i);
            }
            Assert.AreEqual(3000, Convert.ToInt32(duration));
        }

        [TestCase(BezierEasingType.EaseInCubic)]
        [TestCase(BezierEasingType.EaseOutCubic)]
        [TestCase(BezierEasingType.EaseInOutCubic)]
        [TestCase(BezierEasingType.EaseInQuart)]
        [TestCase(BezierEasingType.EaseOutQuart)]
        [TestCase(BezierEasingType.EaseInOutQuart)]
        public void BezierPositionMapper_GetPointOnBezierCurve_BezierEasingType(BezierEasingType bezierEasingType)
        {
            // Arrange
            uint amount = 60;

            // Act
            var mapper = new BezierPositionMapper(amount, bezierEasingType);

            // Assert
            Assert.AreEqual(new Tuple<float, float>(0, 0), mapper.GetPointOnBezierCurve(0));
            Assert.AreEqual(new Tuple<float, float>(1, 1), mapper.GetPointOnBezierCurve(1));

            float duration = 0;
            for (uint i = 0; i < amount; i++)
            {
                duration += mapper.GetMappedPosition(i);
            }
            Assert.IsTrue(duration > 2800f);
            Assert.IsTrue(duration < 3200f);
        }
    }
}

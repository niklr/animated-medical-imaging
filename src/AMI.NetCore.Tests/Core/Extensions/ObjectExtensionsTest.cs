using AMI.Core.Enums;
using AMI.Core.Extensions.ObjectExtensions;
using AMI.Core.Models;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Extensions
{
    [TestFixture]
    public class ObjectExtensionsTest
    {
        [Test]
        public void NetCore_ObjectExtensions_DeepClone()
        {
            // Arrange
            var expected = new ExtractInput()
            {
                AmountPerAxis = 10,
                SourcePath = "test.txt"
            };
            expected.AxisTypes.Add(AxisType.Z);

            // Act
            var actual = expected.DeepClone();

            // Assert
            Assert.AreNotEqual(expected, actual);
            Assert.AreEqual(expected.AmountPerAxis, actual.AmountPerAxis);
            Assert.AreEqual(expected.SourcePath, actual.SourcePath);
            Assert.IsFalse(ReferenceEquals(expected.SourcePath, actual.SourcePath));
            Assert.IsFalse(ReferenceEquals(expected.AxisTypes, actual.AxisTypes));
            Assert.AreEqual(expected.AxisTypes, actual.AxisTypes);
            Assert.AreEqual(expected.AxisTypes.Count, actual.AxisTypes.Count);
        }
    }
}
